using EventLogistics.Domain.Entities;
using EventLogistics.Application.DTOs;

public static class ParticipantMapper
{
    public static ParticipantResponseDto ToResponseDto(Participant participant)
    {
        return new ParticipantResponseDto
        {
            Id = participant.Id,
            FullName = participant.FullName,
            AccessType = participant.AccessType ?? string.Empty,
            RegistrationComplete = participant.IsRegistrationComplete
        };
    }
}