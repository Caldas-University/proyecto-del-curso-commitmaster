using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventLogistics.Domain.Entities
{
    public class Activity : BaseEntity
    {
        [Required]
        public required string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public string? Location { get; set; }

        // Relación con el evento
        [Required]
        public int EventId { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual Event? Event { get; set; }

        // Relación con participantes (n a n)
        public virtual ICollection<ParticipantActivity> ParticipantActivities { get; set; } = new List<ParticipantActivity>();
    }
}
