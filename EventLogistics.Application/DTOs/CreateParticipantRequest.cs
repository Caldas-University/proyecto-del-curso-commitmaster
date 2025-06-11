namespace EventLogistics.Application.DTOs;

public class CreateParticipantRequest
{
    public string Name { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AccessType { get; set; } = string.Empty;
}
