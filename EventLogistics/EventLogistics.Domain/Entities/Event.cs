using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventLogistics.Domain.Entities
{
    // Event entity as shown in the diagram
    public class Event : BaseEntity
    {
        public string Place { get; set; }
        public DateTime Schedule { get; set; }
        public string Status { get; set; }
        
        // Navigation properties
        public virtual ICollection<ResourceAssignment> Resources { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
    }
}