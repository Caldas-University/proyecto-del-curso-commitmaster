using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventLogistics.Domain.Entities
{
    // Notification entity as shown in the diagram
    public class Notification : BaseEntity
    {
        public int RecipientId { get; set; }
        [ForeignKey("RecipientId")]
        public virtual User Recipient { get; set; }
        
        public string Content { get; set; }
        public string Status { get; set; } // e.g., "Sent", "Delivered", "Read"
        public DateTime Timestamp { get; set; }
        public bool Confirmation { get; set; }
    }
}