namespace EventLogistics.Domain.Entities;

public class Attendance
{
    public Guid Id { get; private set; }
    public Guid ParticipantId { get; private set; }
    public Guid EventId { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string Method { get; private set; } // QR o Manual
    public string? QRCode { get; set; } // Opcional, si se genera un QR

    private Attendance() {
        // Constructor privado para EF Core
        //qr
        Id = Guid.NewGuid();
        ParticipantId = Guid.Empty; // Inicialmente vacío
        EventId = Guid.Empty; // Inicialmente vacío
        Method = string.Empty; // Inicialmente vacío
     }

    public Attendance(Guid participantId, Guid eventId, string method)
    {
        Id = Guid.NewGuid();
        ParticipantId = participantId;
        EventId = eventId;
        Timestamp = DateTime.UtcNow;
        Method = method;
        QRCode = null; // Inicialmente no hay QR
    }
}