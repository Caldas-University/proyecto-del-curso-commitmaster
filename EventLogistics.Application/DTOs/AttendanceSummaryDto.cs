namespace EventLogistics.Application.DTOs;

public class AttendanceSummaryDto
{
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public int TotalRegistered { get; set; }
    public int TotalAttended { get; set; }
    public double AttendancePercentage 
        => TotalRegistered == 0 ? 0 : (double)TotalAttended / TotalRegistered * 100;
}