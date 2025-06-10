using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;

public interface IActivityRepository : IRepository<Activity>
{
    Task<IEnumerable<Activity>> GetByEventIdAsync(Guid eventId);
    Task<IEnumerable<Activity>> GetConflictingActivitiesAsync(Guid eventId, DateTime startTime, DateTime endTime, Guid? excludeActivityId = null);
}