using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventLogistics.Domain.Entities
{
    // Location entity for representing event locations
    public class Location : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        [Required]
        public bool State { get; set; } // true = disponible, false = no disponible


        // Relación: Una ubicación puede tener muchas asignaciones de recursos
        public virtual ICollection<ResourceAssignment> ResourceAssignments { get; set; }
    }
}