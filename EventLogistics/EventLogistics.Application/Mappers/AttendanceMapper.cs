using EventLogistics.Domain.Entities;
using EventLogistics.Application.DTOs;

namespace EventLogistics.Application.Mappers;

public static class AttendanceMapper
{
    public static AttendanceDto ToDto(Attendance attendance)
    {
        return new AttendanceDto
        {
            Id = attendance.Id,
            ParticipantId = attendance.ParticipantId,
            EventId = attendance.EventId,
            Timestamp = attendance.Timestamp,
            Method = attendance.Method,
            QRCode = attendance.QRCode // <-- Asegúrate de mapear esto
        };
    }

    public static Attendance ToEntity(AttendanceDto dto)
    {
        // Este constructor puede variar según tu lógica de dominio
        return new Attendance(dto.ParticipantId, dto.EventId, dto.Method);
    }
}