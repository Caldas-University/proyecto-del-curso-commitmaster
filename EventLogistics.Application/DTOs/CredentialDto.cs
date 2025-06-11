namespace EventLogistics.Application.DTOs;

public class CredentialDto
{
    public string ParticipantName { get; set; }
    public string AccessType { get; set; }
    public string EventName { get; set; }
    public string QRCode { get; set; }
    public List<ScheduleItemDto> Schedule { get; set; }
}