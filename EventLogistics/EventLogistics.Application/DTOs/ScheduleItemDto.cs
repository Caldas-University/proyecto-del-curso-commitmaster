namespace EventLogistics.Application.DTOs;

public class ScheduleItemDto
{
    public string ActivityName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Lugar { get; set; } = string.Empty;
}