namespace EventLogistics.Application.DTOs;

public class CreateEventRequest
{
    public string Name { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public DateTime Schedule { get; set; }
    public List<Guid> Resources { get; set; } = new();
    public string Status { get; set; } = string.Empty;
    public Guid LocationId { get; set; }
}
