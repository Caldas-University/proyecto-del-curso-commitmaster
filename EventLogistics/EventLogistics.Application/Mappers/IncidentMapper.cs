using EventLogistics.Domain.Entities;
using EventLogistics.Application.DTOs;

namespace EventLogistics.Application.Mappers;

public static class IncidentMapper
{
    public static IncidentDto ToDto(Incident incident)
    {
        return new IncidentDto
        {
            Id = incident.Id,
            EventId = incident.EventId,
            Description = incident.Description,
            IncidentDate = incident.IncidentDate,
            Location = incident.Location,
            Status = incident.Status
        };
    }

    public static Incident ToEntity(IncidentDto dto)
    {
        return new Incident
        {
            Id = dto.Id,
            EventId = dto.EventId,
            Description = dto.Description,
            IncidentDate = dto.IncidentDate,
            Location = dto.Location,
            Status = dto.Status
        };
    }
}
