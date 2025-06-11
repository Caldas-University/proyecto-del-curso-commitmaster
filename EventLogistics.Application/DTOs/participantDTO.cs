namespace EventLogistics.Application.DTOs;

public class ParticipantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AccessType { get; set; } = string.Empty; // Asistente, Ponente, VIP, etc.
}