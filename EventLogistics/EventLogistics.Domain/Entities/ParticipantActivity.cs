using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventLogistics.Domain.Entities;

namespace EventLogistics.Domain.Entities;

    public class ParticipantActivity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ParticipantId { get; set; }

        [ForeignKey("ParticipantId")]
        public virtual Participant? Participant { get; set; }

        [Required]
        public int ActivityId { get; set; }

        [ForeignKey("ActivityId")]
        public virtual Activity? Activity { get; set; }

        public string? Status { get; set; } // Ej: inscrito, cancelado
    }
