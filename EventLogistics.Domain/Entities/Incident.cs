using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventLogistics.Domain.Entities
{
    public class Incident
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string Description { get; set; }
        public DateTime IncidentDate { get; set; }
        public string Location { get; set; }

        public string Status { get; set; } = "Open"; // Default status is Open

        [System.Text.Json.Serialization.JsonIgnore]
        public Event? Event { get; set; } // Navigation property to the Event entity

    }
}