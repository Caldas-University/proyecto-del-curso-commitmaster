namespace EventLogistics.Application.DTOs;

/// <summary>
/// Datos para la generación de la escarapela y cronograma personalizado.
/// </summary>
public class CredentialDto
{
    public string ParticipantName { get; set; } = string.Empty;
    public string AccessType { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public string QRCode { get; set; } = string.Empty;
    public List<ScheduleItemDto> Schedule { get; set; } = new();
}