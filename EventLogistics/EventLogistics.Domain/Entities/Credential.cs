using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventLogistics.Domain.Entities
{
    public class Credential : BaseEntity
    {
        [Required]
        public int ParticipantId { get; set; }

        [ForeignKey("ParticipantId")]
        public virtual Participant? Participant { get; set; }

        [Required]
        public required string AccessType { get; set; } // Ej: Asistente, Ponente, VIP

        [Required]
        public DateTime IssuedAt { get; set; }

        public bool Printed { get; set; } = false;

        public string? BadgeData { get; set; } // Datos extra para escarapela (QR, nombre, etc)
    }
}
