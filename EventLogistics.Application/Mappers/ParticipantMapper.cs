using EventLogistics.Domain.Entities;
using EventLogistics.Application.DTOs;

namespace EventLogistics.Application.Mappers;

public static class ParticipantMapper
{
    /// <summary>
    /// Convierte una entidad Participant a su DTO.
    /// </summary>
    public static ParticipantDto ToDto(Participant participant)
    {
        if (participant == null) throw new ArgumentNullException(nameof(participant));
        return new ParticipantDto
        {
            Id = participant.Id,
            Name = participant.Name,
            Document = participant.Document,
            Email = participant.Email,
            AccessType = participant.AccessType
        };
    }

    /// <summary>
    /// Convierte un DTO de Participant a su entidad.
    /// </summary>
    public static Participant ToEntity(ParticipantDto dto)
    {
        return new Participant(dto.Name, dto.Document, dto.Email, dto.AccessType);
    }
}