namespace EventLogistics.Application.DTOs;

/// <summary>
/// Representa el registro de asistencia de un participante en un evento.
/// </summary>
public class AttendanceDto
{
    /// <summary>
    /// Identificador único del registro de asistencia.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador del participante.
    /// </summary>
    public Guid ParticipantId { get; set; }

    /// <summary>
    /// Identificador del evento.
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    /// Fecha y hora del registro de asistencia.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Método de registro (QR o Manual).
    /// </summary>
    public string Method { get; set; } = string.Empty;

    /// <summary>
    /// Código QR generado (opcional).
    /// </summary>
    public string? QRCode { get; set; }
}