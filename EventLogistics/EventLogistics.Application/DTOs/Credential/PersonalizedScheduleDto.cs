namespace EventLogistics.Application.DTOs
{
    public class PersonalizedScheduleDto
    {
        public string ParticipantName { get; set; } = string.Empty;
        public List<ActivityScheduleDto> Activities { get; set; } = new();
    }

}

