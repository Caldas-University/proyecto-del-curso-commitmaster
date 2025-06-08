using EventLogistics.Domain.Entities;
using EventLogistics.Infrastructure.Persistence;
using EventLogistics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class ActivityRepository : Repository<Activity>, IActivityRepository
{
    public ActivityRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Activity>> GetByEventId(int eventId)
    {
        return await _context.Activities
            .Where(a => a.EventId == eventId)
            .ToListAsync();
    }

    public Task<IEnumerable<Activity>> GetByEventIdAsync(int eventId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Activity>> GetConflictingActivities(int eventId, DateTime startTime, DateTime endTime, int? excludeActivityId = null)
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

    public Task<IEnumerable<Activity>> GetConflictingActivitiesAsync(int eventId, DateTime startTime, DateTime endTime, int? excludeActivityId = null)
    {
        throw new NotImplementedException();
    }
}