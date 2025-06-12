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