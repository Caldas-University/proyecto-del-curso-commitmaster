using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventLogistics.Domain.Entities;

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

        /*
            Entidad	       Relación   Otra Entidad
            Event	    1 ----------- n	Activity
            Event	    1 ----------- n	Participant
            Participant	n ----------- n	Activity
        */
        public ICollection<Activity> Activities { get; set; }
        public ICollection<Participant> Participants { get; set; }
    }
}