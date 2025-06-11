namespace EventLogistics.Application.DTOs;

public class AttendanceDto
{
    public Guid Id { get; set; }
    public Guid ParticipantId { get; set; }
    public Guid EventId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Method { get; set; } = string.Empty; // QR o Manual
    public string? QRCode { get; set; } = string.Empty;

    public void UpdateParticipantId(Guid participantId)
    {
        ParticipantId = participantId;
    }
}