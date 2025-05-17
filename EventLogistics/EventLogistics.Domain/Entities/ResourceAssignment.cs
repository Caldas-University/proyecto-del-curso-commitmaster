using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventLogistics.Domain.Entities
{
    // Resource Assignment entity for connecting resources to events
    public class ResourceAssignment : BaseEntity
    {
        public int ResourceId { get; set; }
        [ForeignKey("ResourceId")]
        public virtual Resource Resource { get; set; }
        
        public int EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
        
        public int? AssignedToUserId { get; set; }
        [ForeignKey("AssignedToUserId")]
        public virtual User AssignedTo { get; set; }
        
        public DateTime AssignmentStart { get; set; }
        public DateTime AssignmentEnd { get; set; }
        public string Status { get; set; } // e.g., "Assigned", "Completed", "Cancelled"
    }
}