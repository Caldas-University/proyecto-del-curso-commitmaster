namespace EventLogistics.Application.DTOs
{
    public class ActivityScheduleDto
    {
        public string ActivityName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
