using System.Collections.Generic;
using System.Threading.Tasks;
using EventLogistics.Application.DTOs;

public interface IAttendanceService
{
    Task<(bool success, string message, int participantId, string participantName, string accessType)>
        RegisterAttendanceAsync(string? qrCode = null);

    Task<(bool success, string message, int participantId, string participantName, string accessType)>
        RegisterAttendanceManuallyAsync(string document, string name);

    Task<IEnumerable<AttendanceResponseDto>> GetAttendanceByParticipantAsync(int participantId);

    Task<IEnumerable<AttendanceResponseDto>> GetAttendanceByEventAsync(int eventId);

    Task<bool> GetCheckInStatusAsync(int participantId);

    Task<(bool success, string message)> RegularizeAccessAsync(int participantId, Dictionary<string, string> dataToUpdate);
}