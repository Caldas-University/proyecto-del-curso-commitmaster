namespace EventLogistics.Application.DTOs
{
    public class ActivityDto
    {
        public Guid? Id { get; set; } // Opcional para creación
        public string Name { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "Programada";
        public Guid EventId { get; set; }
        public Guid OrganizatorId { get; set; }
    }
}
