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
<<<<<<< HEAD
        Content = string.Empty;
        Status = string.Empty;
=======
        public int RecipientId { get; set; }
        [ForeignKey("RecipientId")]
        public virtual User Recipient { get; set; }

        public string Content { get; set; }
        public string Status { get; set; } // e.g., "Sent", "Delivered", "Read"
        public DateTime Timestamp { get; set; }
        public bool Confirmation { get; set; }
        public string Channel { get; set; } // "Email", "SMS", etc.
        public string NotificationType { get; set; } // "ResourceReassigned", etc.
        public int? RelatedAssignmentId { get; set; }
        [ForeignKey("RelatedAssignmentId")]
        public virtual ResourceAssignment RelatedAssignment { get; set; }
>>>>>>> sebas
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
