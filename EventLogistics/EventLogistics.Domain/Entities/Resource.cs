using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventLogistics.Domain.Entities
{
    // Resource entity as shown in the diagram
    public class Resource : BaseEntity
    {
        public string Type { get; set; }
        public bool Availability { get; set; }
        public int Capacity { get; set; }
        
        // Navigation properties
        public virtual ICollection<ResourceAssignment> Assignments { get; set; }
        public virtual ICollection<ReassignmentRule> ReassignmentRules { get; set; }
    }
}