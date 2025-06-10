using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventLogistics.Domain.Entities
{
    public class IncidentSolution
    {
        public Guid Id { get; set; }
        public Guid IncidentId { get; set; }
        public string ActionTaken { get; set; }
        public string AppliedBy { get; set; }
        public DateTime DateApplied { get; set; }

        public Incident Incident { get; set; }
    }

}

