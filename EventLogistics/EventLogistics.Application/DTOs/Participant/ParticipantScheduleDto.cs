namespace EventLogistics.Application.DTOs
{
    public class ParticipantScheduleDto
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public List<ActivityScheduleItemDto> Activities { get; set; } = new List<ActivityScheduleItemDto>();
    }

    public class ActivityScheduleItemDto
    {
        public string? ActivityName { get; set; }
        public string? Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
