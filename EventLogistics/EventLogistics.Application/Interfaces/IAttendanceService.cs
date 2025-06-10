using EventLogistics.Application.DTOs;

namespace EventLogistics.Application.Interfaces;

public interface IAttendanceServiceApp
{
    Task<AttendanceDto> RegisterAttendanceAsync(Guid participantId, Guid eventId, string method);
    Task<AttendanceDto> RegisterAttendanceByDocumentAsync(string document, Guid eventId, string method);
    Task<CredentialDto> GenerateCredentialAsync(Guid participantId, Guid eventId);
    Task<List<AttendanceDto>> GetAttendanceByParticipantAsync(Guid participantId, Guid eventId);
}