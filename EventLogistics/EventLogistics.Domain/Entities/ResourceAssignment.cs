using System.ComponentModel.DataAnnotations.Schema;

namespace EventLogistics.Domain.Entities
{
    public class ResourceAssignment : BaseEntity
    {
        public int ResourceId { get; set; }
        [ForeignKey("ResourceId")]
        public virtual Resource Resource { get; set; }

        public int EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
        public int? ActivityId { get; set; } 
        public Activity? Activity { get; set; }
        public int? AssignedToUserId { get; set; }
        [ForeignKey("AssignedToUserId")]
        public virtual User AssignedTo { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime AssignmentDate { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } // e.g., "Assigned", "Completed", "Cancelled"
        [Column("IsModified")]
        public bool IsModified { get; set; } = false;
        
        [Column("OriginalAssignmentId")]
        public int? OriginalAssignmentId { get; set; } // Para rastrear reasignaciones
        
        [Column("ModificationReason")]
        public string ModificationReason { get; set; } // Ej: "Stock insuficiente", "Cambio de horario"
    }
}