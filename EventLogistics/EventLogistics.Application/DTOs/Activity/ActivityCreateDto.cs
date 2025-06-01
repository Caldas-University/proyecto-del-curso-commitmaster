namespace EventLogistics.Application.DTOs
{
    public class ActivityCreateDto
    {
        public string ActivityName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EventId { get; set; }
    }
}