using EventLogistics.Application.DTOs;
using EventLogistics.Domain.Entities;

namespace EventLogistics.Application.Mappers;

public static class CredentialMapper
{
    /// <summary>
    /// Convierte los datos de participante y evento en un CredentialDto.
    /// </summary>
    /// <param name="participant">Entidad participante.</param>
    /// <param name="eventName">Nombre del evento.</param>
    /// <param name="qrCode">Código QR generado.</param>
    /// <param name="schedule">Cronograma personalizado.</param>
    /// <returns>DTO de la credencial.</returns>
    public static CredentialDto ToDto(Participant participant, string eventName, string? qrCode, List<ScheduleItemDto> schedule)
    {
        if (participant == null) throw new ArgumentNullException(nameof(participant));
        if (string.IsNullOrWhiteSpace(eventName)) throw new ArgumentException("El nombre del evento es requerido.", nameof(eventName));
        if (schedule == null) throw new ArgumentNullException(nameof(schedule));

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