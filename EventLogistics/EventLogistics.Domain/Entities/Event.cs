using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventLogistics.Domain.Entities
{
    // Event entity as shown in the diagram
    public class Event : BaseEntity
    
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LocationId { get; set; }

        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]

        public DateTime EndTime { get; set; }

        public string Description { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public bool Status { get; set; }
        public virtual ICollection<ResourceAssignment> Resources { get; set; } = new List<ResourceAssignment>();
        // Navigation properties
    }
}