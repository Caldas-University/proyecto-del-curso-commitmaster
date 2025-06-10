namespace EventLogistics.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid RecipientId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendiente";
    public DateTime Timestamp { get; set; }
    public bool Confirmation { get; set; }
    
    // Propiedades adicionales que necesitan algunos servicios
    public string Channel { get; set; } = "Email"; // Email, SMS, Push, etc.
    public string NotificationType { get; set; } = "General"; // General, ResourceAssignment, etc.

    public Notification()
    {
        Content = string.Empty;
        Status = "Pendiente";
        Timestamp = DateTime.UtcNow;
        Confirmation = false;
        Channel = "Email";
        NotificationType = "General";
    }

    public Notification(Guid recipientId, string content, string channel = "Email", string notificationType = "General")
    {
        RecipientId = recipientId;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        Status = "Pendiente";
        Timestamp = DateTime.UtcNow;
        Confirmation = false;
        Channel = channel ?? "Email";
        NotificationType = notificationType ?? "General";
    }
}
