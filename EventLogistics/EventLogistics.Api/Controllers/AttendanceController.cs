using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventLogistics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceServiceApp _attendanceService;

    public AttendanceController(IAttendanceServiceApp attendanceService)
    {
        _attendanceService = attendanceService;
    }

    /// <summary>
    /// Registrar asistencia por participantId y eventId (general, no recomendado para uso manual)
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<AttendanceDto>> RegisterAttendance([FromBody] RegisterAttendanceRequest request)
    {
        if (request.ParticipantId == Guid.Empty || request.EventId == Guid.Empty)
            return BadRequest(new { error = "ParticipantId y EventId son obligatorios." });

        try
        {
            var attendance = await _attendanceService.RegisterAttendanceAsync(request.ParticipantId, request.EventId, request.Method);
            return Ok(attendance);
        }        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("no existe"))
                return NotFound(new { error = ex.Message });
            if (ex.Message.Contains("no está inscrito"))
                return Conflict(new { error = ex.Message, regularizar = true });
            if (ex.Message.Contains("ya fue registrada"))
                return Conflict(new { error = ex.Message });
            return BadRequest(new { error = ex.Message });
        }
    }    /// <summary>
    /// Registrar asistencia manual por documento y eventId
    /// </summary>
    [HttpPost("register-manual")]
    public async Task<ActionResult<AttendanceDto>> RegisterManual([FromBody] RegisterManualRequest request)
    {
        try
        {
            var attendance = await _attendanceService.RegisterAttendanceByDocumentAsync(request.Document, request.EventId, "Manual");
            return Ok(attendance);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("no encontrado"))
                return NotFound(new { error = ex.Message });
            if (ex.Message.Contains("no está inscrito"))
                return Conflict(new { error = ex.Message });
            if (ex.Message.Contains("ya fue registrada"))
                return Conflict(new { error = ex.Message });
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Registrar asistencia por QR (el QR debe contener participantId|eventId|timestamp)
    /// </summary>
    [HttpPost("register-qr")]
    public async Task<ActionResult<AttendanceDto>> RegisterQr([FromBody] RegisterQrRequest request)
    {
        var parts = request.QrContent.Split('|');
        if (parts.Length < 3 || !Guid.TryParse(parts[0], out var participantId) || !Guid.TryParse(parts[1], out var eventId) || !DateTime.TryParseExact(parts[2], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var timestamp))
            return BadRequest("QR inválido o sin timestamp.");

        if ((DateTime.UtcNow - timestamp).TotalMinutes > 10)
            return BadRequest("QR expirado.");

        var attendance = await _attendanceService.RegisterAttendanceAsync(participantId, eventId, "QR");
        return Ok(attendance);
    }

    /// <summary>
    /// Obtener credencial personalizada con QR y cronograma
    /// </summary>
    [HttpGet("credential/{participantId}/{eventId}")]
    public async Task<ActionResult<CredentialDto>> GetCredential(Guid participantId, Guid eventId)
    {
        var credential = await _attendanceService.GenerateCredentialAsync(participantId, eventId);
        return Ok(credential);
    }

    /// <summary>
    /// Consultar asistencias de un participante en un evento
    /// </summary>
    [HttpGet("participant/{participantId}/event/{eventId}")]
    public async Task<ActionResult<List<AttendanceDto>>> GetAttendance(Guid participantId, Guid eventId)
    {
        var attendances = await _attendanceService.GetAttendanceByParticipantAsync(participantId, eventId);
        return Ok(attendances);
    }

    /// <summary>
    /// Consultar cronograma de un participante en un evento
    /// </summary>
    [HttpGet("schedule/{participantId}/{eventId}")]
    public async Task<ActionResult<List<ScheduleItemDto>>> GetSchedule(Guid participantId, Guid eventId)
    {
        var credential = await _attendanceService.GenerateCredentialAsync(participantId, eventId);
        return Ok(credential.Schedule);
    }
}

// Requests para los endpoints
public class RegisterAttendanceRequest
{
    public Guid ParticipantId { get; set; }
    public Guid EventId { get; set; }
    public string Method { get; set; } = string.Empty; // QR o Manual
}

public class RegisterManualRequest
{
    public string Document { get; set; } = string.Empty;
    public Guid EventId { get; set; }
}

public class RegisterQrRequest
{
    public string QrContent { get; set; } = string.Empty;
}