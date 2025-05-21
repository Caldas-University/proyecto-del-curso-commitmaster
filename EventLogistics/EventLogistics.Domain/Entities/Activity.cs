namespace EventLogistics.Domain.Entities
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public required string Name { get; set; }
        public DateTime Fecha { get; set; }
        public int OrganizatorId { get; set; }
        public required Organizator Organizator { get; set; }
    }
}