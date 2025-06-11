namespace EventLogistics.Domain.Entities
{
    public class Activity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "Programada";
        
        // Relaciones
        public Guid EventId { get; set; }
        public virtual Event Event { get; set; }
        
        public Guid OrganizatorId { get; set; }
        public virtual Organizator Organizator { get; set; }
        
        public virtual ICollection<ResourceAssignment> ResourceAssignments { get; set; } = new List<ResourceAssignment>();
        
        public Activity()
        {
            Name = string.Empty;
            Place = string.Empty;
            Status = "Programada";
        }

        public Activity(Guid eventId, Guid organizatorId, string name, string place, DateTime startTime, DateTime endTime, string status = "Programada")
        {
            EventId = eventId;
            OrganizatorId = organizatorId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Place = place ?? throw new ArgumentNullException(nameof(place));
            StartTime = startTime;
            EndTime = endTime;
            Status = status ?? throw new ArgumentNullException(nameof(status));
        }
    }
}