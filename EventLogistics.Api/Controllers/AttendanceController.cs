using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventLogistics.Api.Controllers;

/// <summary>
/// Controlador para el registro de asistencia y generación de credenciales.
/// Permite a los organizadores validar la llegada de los asistentes el día del evento,
/// entregarles su escarapela identificativa y un cronograma personalizado de actividades.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceServiceApp _attendanceService;

    /// <summary>
    /// Constructor del controlador de asistencia.
    /// </summary>
    /// <param name="attendanceService">Servicio de aplicación para operaciones de asistencia.</param>
    public AttendanceController(IAttendanceServiceApp attendanceService)
    {
        _attendanceService = attendanceService;
    }

    /// <summary>
    /// Registrar asistencia manual por documento y evento.
    /// Una vez confirmada la asistencia, el sistema genera e imprime automáticamente una escarapela PDF
    /// con los datos del participante y un cronograma personalizado de actividades.
    /// Si el participante no está inscrito correctamente, se alerta y permite regularización en tiempo real.
    /// </summary>
    /// <param name="request">Documento y evento del participante.</param>
    /// <returns>Archivo PDF de la escarapela o mensaje de error.</returns>
    [HttpPost("register-manual")]
    public async Task<IActionResult> RegisterManual([FromBody] RegisterManualRequest request)
    {
        try
        {
            var attendance = await _attendanceService.RegisterAttendanceManualAsync(request.Document, request.EventId);
            var pdfBytes = await _attendanceService.GenerateCredentialAndSchedulePdfAsync(attendance.ParticipantId, attendance.EventId);
            return File(pdfBytes, "application/pdf", $"Escarapela_{attendance.ParticipantId}_{attendance.EventId}.pdf");
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("no encontrado") || ex.Message.Contains("no existe"))
                return NotFound(new { error = ex.Message });
            if (ex.Message.Contains("no está inscrito"))
                return Conflict(new { error = ex.Message, regularizar = true });
            if (ex.Message.Contains("ya fue registrada"))
                return Conflict(new { error = ex.Message });
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Registrar asistencia por QR (el QR debe contener participantId|eventId|timestamp).
    /// Una vez confirmada la asistencia, el sistema genera e imprime automáticamente una escarapela PDF
    /// con los datos del participante y un cronograma personalizado de actividades.
    /// Si el participante no está inscrito correctamente, se alerta y permite regularización en tiempo real.
    /// </summary>
    /// <param name="request">Contenido del QR escaneado.</param>
    /// <returns>Archivo PDF de la escarapela o mensaje de error.</returns>
    [HttpPost("register-qr")]
    public async Task<IActionResult> RegisterQr([FromBody] RegisterQrRequest request)
    {
        var parts = request.QrContent.Split('|');
        if (parts.Length < 3 || !Guid.TryParse(parts[0], out var participantId) || !Guid.TryParse(parts[1], out var eventId) || !DateTime.TryParseExact(parts[2], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var timestamp))
            return BadRequest(new { error = "QR inválido o sin timestamp." });

        if ((DateTime.UtcNow - timestamp).TotalMinutes > 10)
            return BadRequest(new { error = "QR expirado." });

        try
        {
            var attendance = await _attendanceService.RegisterAttendanceAsync(participantId, eventId, "QR");
            var pdfBytes = await _attendanceService.GenerateCredentialAndSchedulePdfAsync(participantId, eventId);
            return File(pdfBytes, "application/pdf", $"Escarapela_{participantId}_{eventId}.pdf");
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("no existe"))
                return NotFound(new { error = ex.Message });
            if (ex.Message.Contains("no está inscrito"))
                return Conflict(new { error = ex.Message, regularizar = true });
            if (ex.Message.Contains("ya fue registrada"))
                return Conflict(new { error = ex.Message });
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener el contenido QR para un participante y evento (antes del evento, para enviar o mostrar el QR).
    /// </summary>
    /// <param name="participantId">ID del participante.</param>
    /// <param name="eventId">ID del evento.</param>
    /// <returns>Contenido del QR en string.</returns>
    [HttpGet("qr-content")]
    public async Task<IActionResult> GetQrContent([FromQuery] Guid participantId, [FromQuery] Guid eventId)
    {
        try
        {
            var qrContent = await _attendanceService.GetQrContentAsync(participantId, eventId);
            return Ok(new { qrContent });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener el PDF de la escarapela (credencial) para un participante y evento.
    /// </summary>
    /// <param name="participantId">ID del participante.</param>
    /// <param name="eventId">ID del evento.</param>
    /// <returns>Archivo PDF de la escarapela o mensaje de error.</returns>
    [HttpGet("credential-pdf")]
    public async Task<IActionResult> GetCredentialPdf([FromQuery] Guid participantId, [FromQuery] Guid eventId)
    {
        try
        {
            var pdfBytes = await _attendanceService.GenerateCredentialAndSchedulePdfAsync(participantId, eventId);
            return File(pdfBytes, "application/pdf", $"Escarapela_{participantId}_{eventId}.pdf");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Listar todas las asistencias registradas.
    /// </summary>
    /// <returns>Lista de asistencias.</returns>
    [HttpGet("list")]
    public async Task<IActionResult> ListAll()
    {
        try
        {
            var list = await _attendanceService.ListAllAsync();
            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener una asistencia por su ID.
    /// </summary>
    /// <param name="attendanceId">ID de la asistencia.</param>
    /// <returns>Detalle de la asistencia.</returns>
    [HttpGet("{attendanceId}")]
    public async Task<IActionResult> GetById([FromRoute] Guid attendanceId)
    {
        try
        {
            var attendance = await _attendanceService.GetByIdAsync(attendanceId);
            if (attendance == null)
                return NotFound(new { error = "Asistencia no encontrada." });
            return Ok(attendance);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Listar asistencias por evento.
    /// </summary>
    /// <param name="eventId">ID del evento.</param>
    /// <returns>Lista de asistencias del evento.</returns>
    [HttpGet("by-event")]
    public async Task<IActionResult> ListByEvent([FromQuery] Guid eventId)
    {
        var list = await _attendanceService.ListByEventAsync(eventId);
        return Ok(list);
    }

    /// <summary>
    /// Listar asistencias por participante.
    /// </summary>
    /// <param name="participantId">ID del participante.</param>
    /// <returns>Lista de asistencias del participante.</returns>
    [HttpGet("by-participant")]
    public async Task<IActionResult> ListByParticipant([FromQuery] Guid participantId)
    {
        var list = await _attendanceService.ListByParticipantAsync(participantId);
        return Ok(list);
    }

    /// <summary>
    /// Obtener el cronograma de actividades para un participante y evento.
    /// </summary>
    /// <param name="participantId">ID del participante.</param>
    /// <param name="eventId">ID del evento.</param>
    /// <returns>Detalle del cronograma de actividades.</returns>
    [HttpGet("schedule")]
    public async Task<IActionResult> GetSchedule([FromQuery] Guid participantId, [FromQuery] Guid eventId)
    {
        var schedule = await _attendanceService.GetScheduleAsync(participantId, eventId);
        return Ok(schedule);
    }

    /// <summary>
    /// Verificar inscripción de un participante a un evento.
    /// </summary>
    /// <param name="participantId">ID del participante.</param>
    /// <param name="eventId">ID del evento.</param>
    /// <returns>Estado de la inscripción.</returns>
    [HttpGet("verify-inscription")]
    public async Task<IActionResult> VerifyInscription([FromQuery] Guid participantId, [FromQuery] Guid eventId)
    {
        var isInscribed = await _attendanceService.VerifyInscriptionAsync(participantId, eventId);
        return Ok(new { isInscribed });
    }

    /// <summary>
    /// Regularizar la inscripción de un participante a un evento.
    /// </summary>
    /// <param name="request">Datos del participante y evento.</param>
    /// <returns>Estado de la regularización.</returns>
    [HttpPost("regularize-inscription")]
    public async Task<IActionResult> RegularizeInscription([FromBody] RegularizeInscriptionRequest request)
    {
        await _attendanceService.RegularizeInscriptionAsync(request.ParticipantId, request.EventId);
        return Ok();
    }

    /// <summary>
    /// Obtener un resumen de la asistencia por evento.
    /// </summary>
    /// <param name="eventId">ID del evento.</param>
    /// <returns>Resumen de la asistencia.</returns>
    [HttpGet("summary")]
    public async Task<IActionResult> GetAttendanceSummary([FromQuery] Guid eventId)
    {
        var summary = await _attendanceService.GetAttendanceSummaryAsync(eventId);
        return Ok(summary);
    }

    /// <summary>
    /// Listar participantes por evento.
    /// </summary>
    /// <param name="eventId">ID del evento.</param>
    /// <returns>Lista de participantes del evento.</returns>
    [HttpGet("participants-by-event")]
    public async Task<IActionResult> ListParticipantsByEvent([FromQuery] Guid eventId)
    {
        var list = await _attendanceService.ListParticipantsByEventAsync(eventId);
        return Ok(list);
    }
}

/// <summary>
/// Modelo para registrar asistencia manual.
/// </summary>
public class RegisterManualRequest
{
    /// <summary>
    /// Documento de identidad del participante.
    /// </summary>
    public string Document { get; set; } = string.Empty;

    /// <summary>
    /// ID del evento.
    /// </summary>
    public Guid EventId { get; set; }
}

/// <summary>
/// Modelo para registrar asistencia por QR.
/// </summary>
public class RegisterQrRequest
{
    /// <summary>
    /// Contenido del QR escaneado.
    /// </summary>
    public string QrContent { get; set; } = string.Empty;
}

/// <summary>
/// Modelo para regularizar inscripción.
/// </summary>
public class RegularizeInscriptionRequest
{
    /// <summary>
    /// ID del participante.
    /// </summary>
    public Guid ParticipantId { get; set; }

    /// <summary>
    /// ID del evento.
    /// </summary>
    public Guid EventId { get; set; }
}