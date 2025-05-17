using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventLogistics.Domain.Entities
{
    // Reassignment Rule entity for handling resource reallocation
    public class ReassignmentRule : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Condition { get; set; } // Could be a JSON representation of conditions
        public int Priority { get; set; } // Higher priority rules take precedence
        public bool IsActive { get; set; }
        
        // Optional resource type specification
        public int? ResourceTypeId { get; set; }
        [ForeignKey("ResourceTypeId")]
        public virtual Resource ResourceType { get; set; }
        
        // Action to take when rule matches
        public string Action { get; set; } // JSON or string representation of the action
    }
}