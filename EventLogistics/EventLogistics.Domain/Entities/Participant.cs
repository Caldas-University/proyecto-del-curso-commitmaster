using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventLogistics.Domain.Entities
{
    public class Participant : BaseEntity
    {
        [Required]
        public string FullName { get; set; }

        public string AccessType { get; set; }
        
        public string QrCode { get; set; } = null!;   // "null!" para ignorar warning inicial
        
        public string Document { get; set; } = null!;

        [Required]
        public int EventId { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual Event Event { get; set; }

        public virtual ICollection<ParticipantActivity> ParticipantActivities { get; set; } = new List<ParticipantActivity>();

        [NotMapped]
    public bool IsRegistrationComplete => !string.IsNullOrWhiteSpace(FullName)
                                           && !string.IsNullOrWhiteSpace(Document)
                                           && ParticipantActivities != null
                                           && ParticipantActivities.Count != 0;
                                           
    }
}
