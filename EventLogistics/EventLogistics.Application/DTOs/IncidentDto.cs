namespace EventLogistics.Application.DTOs;

public class IncidentDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime IncidentDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
}

public class CreateIncidentRequest
{
    public Guid EventId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime IncidentDate { get; set; }
    public string Location { get; set; } = string.Empty;
}

public class UpdateIncidentRequest
{
    public string Description { get; set; } = string.Empty;
    public DateTime IncidentDate { get; set; }
    public string Location { get; set; } = string.Empty;
}
