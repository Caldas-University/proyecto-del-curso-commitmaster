using EventLogistics.Domain.Entities;
using EventLogistics.Application.DTOs; 

namespace EventLogistics.Application.Mappers
{
    public static class AttendanceMapper
    {
        public static AttendanceResponseDto ToResponseDto(Attendance attendance, Participant participant)
        {
            return new AttendanceResponseDto
            {
                ParticipantId = participant.Id,
                FullName = participant.FullName,
                AccessType = participant.AccessType,
                CheckInTime = attendance.CheckInTime,
                RegistrationComplete = participant.IsRegistrationComplete,
                Message = "Asistencia encontrada"
            };
        }
    }
}