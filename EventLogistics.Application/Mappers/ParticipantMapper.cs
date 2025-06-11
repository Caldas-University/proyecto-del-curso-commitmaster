using EventLogistics.Domain.Entities;
using EventLogistics.Application.DTOs;

namespace EventLogistics.Application.Mappers;

public static class ParticipantMapper
{
    public static ParticipantDto ToDto(Participant participant)
    {
        return new ParticipantDto
        {
            Id = participant.Id,
            Name = participant.Name,
            Document = participant.Document,
            Email = participant.Email,
            AccessType = participant.AccessType
        };
    }

    // Si necesitas crear entidades desde DTOs:
    public static Participant ToEntity(ParticipantDto dto)
    {
        return new Participant(dto.Name, dto.Document, dto.Email, dto.AccessType);
    }
}