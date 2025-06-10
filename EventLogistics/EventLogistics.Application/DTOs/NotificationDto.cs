namespace EventLogistics.Application.DTOs;

public class NotificationDto
{
    public Guid Id { get; set; }
    public Guid RecipientId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public bool Confirmation { get; set; }
}
