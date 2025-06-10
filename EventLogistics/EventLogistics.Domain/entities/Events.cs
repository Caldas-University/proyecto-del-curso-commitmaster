using System;
using System.Collections.Generic;



namespace EventLogistics.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

    //    public DateTime StartDate { get; set; }

        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    }
}

