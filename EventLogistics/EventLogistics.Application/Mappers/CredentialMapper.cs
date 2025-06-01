using EventLogistics.Domain.Entities;
using EventLogistics.Application.DTOs;

public static class CredentialMapper
{
    public static CredentialResponseDto ToResponseDto(Participant participant, byte[] credentialPdf, PersonalizedScheduleDto? schedule)
    {
        return new CredentialResponseDto
        {
            ParticipantName = participant.FullName,
            AccessType = participant.AccessType ?? string.Empty,
            CredentialPdf = credentialPdf,
            Schedule = schedule ?? new PersonalizedScheduleDto()
        };
    }
}