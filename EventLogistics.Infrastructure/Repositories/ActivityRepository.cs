using EventLogistics.Domain.Entities;
using EventLogistics.Infrastructure.Persistence;
using EventLogistics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class ActivityRepository : Repository<Activity>, IActivityRepository
{
    public ActivityRepository(EventLogisticsDbContext context) : base(context) { }    public async Task<IEnumerable<Activity>> GetByEventId(Guid eventId)
    {
        return await _context.Activities
            .Where(a => a.EventId == eventId)
            .ToListAsync();
    }

    public Task<IEnumerable<Activity>> GetByEventIdAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Activity>> GetConflictingActivities(Guid eventId, DateTime startTime, DateTime endTime, Guid? excludeActivityId = null)
    {
        var query = _context.Activities
            .Where(a => a.EventId == eventId && 
                   ((startTime >= a.StartTime && startTime < a.EndTime) ||
                    (endTime > a.StartTime && endTime <= a.EndTime) ||
                    (startTime <= a.StartTime && endTime >= a.EndTime)));

        if (excludeActivityId.HasValue)
        {
            query = query.Where(a => a.Id != excludeActivityId.Value);
        }

        return await query.ToListAsync();
    }

    public Task<IEnumerable<Activity>> GetConflictingActivitiesAsync(Guid eventId, DateTime startTime, DateTime endTime, Guid? excludeActivityId = null)
    {
        throw new NotImplementedException();
    }
}