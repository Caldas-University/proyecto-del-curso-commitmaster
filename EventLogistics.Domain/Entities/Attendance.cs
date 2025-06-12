namespace EventLogistics.Domain.Entities;

public class Attendance
{
    /// <summary>
    /// Identificador único del registro de asistencia.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Identificador del participante.
    /// </summary>
    public Guid ParticipantId { get; private set; }

    /// <summary>
    /// Identificador del evento.
    /// </summary>
    public Guid EventId { get; private set; }

    /// <summary>
    /// Fecha y hora del registro de asistencia.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Método de registro (QR o Manual).
    /// </summary>
    public string Method { get; private set; }

    /// <summary>
    /// Código QR generado (opcional).
    /// </summary>
    public string? QRCode { get; set; }

    private Attendance()
    {
        // Constructor privado para EF Core
        Id = Guid.NewGuid();
        ParticipantId = Guid.Empty;
        EventId = Guid.Empty;
        Method = string.Empty;
    }

    /// <summary>
    /// Constructor para crear un nuevo registro de asistencia.
    /// </summary>
    public Attendance(Guid participantId, Guid eventId, string method)
    {
        Id = Guid.NewGuid();
        ParticipantId = participantId;
        EventId = eventId;
        Timestamp = DateTime.UtcNow;
        Method = method;
        QRCode = null;
    }

    /// <summary>
    /// Constructor para reconstrucción completa (por ejemplo, desde un DTO o la base de datos).
    /// </summary>
    public Attendance(Guid id, Guid participantId, Guid eventId, DateTime timestamp, string method, string? qrCode)
    {
        Id = id;
        ParticipantId = participantId;
        EventId = eventId;
        Timestamp = timestamp;
        Method = method;
        QRCode = qrCode;
    }
}