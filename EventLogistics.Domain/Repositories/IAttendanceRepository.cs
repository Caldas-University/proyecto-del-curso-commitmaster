using EventLogistics.Domain.Entities;

namespace EventLogistics.Domain.Repositories;

public interface IAttendanceRepository
{
    Task<Attendance?> GetByIdAsync(Guid id);
    Task<List<Attendance>> GetByParticipantAsync(Guid participantId, Guid eventId);
    Task AddAsync(Attendance attendance);
    Task<bool> ExistsAsync(Guid participantId, Guid eventId);
}