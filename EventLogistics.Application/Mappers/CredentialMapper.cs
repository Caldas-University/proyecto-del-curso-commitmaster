using EventLogistics.Application.DTOs;
using EventLogistics.Domain.Entities;

namespace EventLogistics.Application.Mappers;

public static class CredentialMapper
{
    public static CredentialDto ToDto(Participant participant, string eventName, string qrCode, List<ScheduleItemDto> schedule)
    {
        return new CredentialDto
        {
            ParticipantName = participant.Name,
            AccessType = participant.AccessType,
            EventName = eventName,
            QRCode = qrCode,
            Schedule = schedule
        };
    }
}