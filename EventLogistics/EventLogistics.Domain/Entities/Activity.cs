namespace EventLogistics.Domain.Entities
{
    public class Activity : BaseEntity
    {
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int OrganizatorId { get; set; }
        public Organizator Organizator { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public ICollection<ResourceAssignment> ResourceAssignments { get; set; }
    }
}