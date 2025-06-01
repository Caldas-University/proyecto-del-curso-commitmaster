namespace EventLogistics.Application.DTOs;
public class BadgeDataDto
{
    public string? ParticipantName { get; set; }
    public string? AccessType { get; set; }
    public string? EventName { get; set; }
    public string? QrCode { get; set; }
    public DateTime? IssuedAt { get; set; }
    public bool Printed { get; set; }
}