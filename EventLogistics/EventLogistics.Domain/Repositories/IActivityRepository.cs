using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;

public interface IActivityRepository : IRepository<Activity>
{
    Task<IEnumerable<Activity>> GetByEventIdAsync(int eventId);
    Task<IEnumerable<Activity>> GetConflictingActivitiesAsync(int eventId, DateTime startTime, DateTime endTime, int? excludeActivityId = null);
}