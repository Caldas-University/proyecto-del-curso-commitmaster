using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventLogistics.Domain.Entities
{
    // NotificationHistory to track all notification activities
    public class NotificationHistory : BaseEntity
    {
        public int NotificationId { get; set; }
        [ForeignKey("NotificationId")]
        public virtual Notification Notification { get; set; }
        
        public string Action { get; set; } // e.g., "Generated", "Sent", "Confirmed"
        public DateTime ActionTimestamp { get; set; }
        public string Details { get; set; } // Additional details about the action
        public string Result { get; set; } // Success/Failure/Other status
    }
}