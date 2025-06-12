using EventLogistics.Domain.Entities;
using EventLogistics.Application.DTOs;

namespace EventLogistics.Application.Mappers;

public static class AttendanceMapper
{
    /// <summary>
    /// Convierte una entidad Attendance a su DTO.
    /// </summary>
    public static AttendanceDto ToDto(Attendance attendance)
    {
        if (attendance == null) throw new ArgumentNullException(nameof(attendance));
        return new AttendanceDto
        {
            Id = attendance.Id,
            ParticipantId = attendance.ParticipantId,
            EventId = attendance.EventId,
            Timestamp = attendance.Timestamp,
            Method = attendance.Method,
            QRCode = attendance.QRCode
        };
    }

    /// <summary>
    /// Convierte un DTO de Attendance a su entidad.
    /// </summary>
    public static Attendance ToEntity(AttendanceDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        var entity = new Attendance(dto.ParticipantId, dto.EventId, dto.Method)
        {
            Timestamp = dto.Timestamp,
            QRCode = dto.QRCode
        };
        return entity;
    }
}