using System;
using System.Collections.Generic;

namespace EventLogistics.Domain.Entities
{    public class Event : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
        public DateTime Schedule { get; set; }
        public string Status { get; set; } = "Activo";
        
        // Foreign Key for Location
        public Guid LocationId { get; set; }
        
        // Navigation properties
        public virtual Location Location { get; set; } = null!;
        public virtual ICollection<ResourceAssignment> Resources { get; set; } = new List<ResourceAssignment>();
        public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
        
        public Event()
        {
            Name = string.Empty;
            Place = string.Empty;
            Status = "Activo";
        }        public Event(string name, string place, DateTime schedule, Guid locationId, string status = "Activo")
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Place = place ?? throw new ArgumentNullException(nameof(place));
            Schedule = schedule;
            LocationId = locationId;
            Status = status ?? throw new ArgumentNullException(nameof(status));
        }
    }
}