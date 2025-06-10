namespace EventLogistics.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; }
    public Guid RecipientId { get; private set; }
    public string Content { get; private set; }
    public string Status { get; private set; }
    public DateTime Timestamp { get; private set; }
    public bool Confirmation { get; private set; }

    private Notification()
    {
        Content = string.Empty;
        Status = string.Empty;
    }

    public Notification(Guid recipientId, string content)
    {
        Id = Guid.NewGuid();
        RecipientId = recipientId;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        Status = "Pendiente";
        Timestamp = DateTime.UtcNow;
        Confirmation = false;
    }
}
