

namespace EventLogistics.Domain.Repositories
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        Task<IEnumerable<Attendance>> GetByParticipantIdAsync(int participantId);
        Task<Attendance> GetByEventAndParticipantAsync(int eventId, int participantId);
        Task<IEnumerable<Attendance>> GetByDateAsync(DateTime date);
    }
}
