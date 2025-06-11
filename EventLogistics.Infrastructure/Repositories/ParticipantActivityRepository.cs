using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventLogistics.Infrastructure.Repositories;

public class ParticipantActivityRepository : IParticipantActivityRepository
{
    private readonly EventLogisticsDbContext _context;

    public ParticipantActivityRepository(EventLogisticsDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AnyAsync(Func<ParticipantActivity, bool> predicate)
    {
        // Nota: Para expresiones complejas, considera usar Expression<Func<...>>
        return await Task.FromResult(_context.ParticipantActivities
            .Include(pa => pa.Activity)
            .Any(predicate));
    }

    public async Task<List<ParticipantActivity>> GetByParticipantAndEventAsync(Guid participantId, Guid eventId)
    {
        return await _context.ParticipantActivities
            .Include(pa => pa.Activity)
            .Where(pa => pa.ParticipantId == participantId && pa.Activity != null && pa.Activity.EventId == eventId)
            .ToListAsync();
    }

    public async Task<List<Activity>> GetAvailableActivitiesForEventAsync(Guid participantId, Guid eventId)
    {
        var inscritas = await _context.ParticipantActivities
            .Where(pa => pa.ParticipantId == participantId && pa.Activity != null && pa.Activity.EventId == eventId)
            .Select(pa => pa.ActivityId)
            .ToListAsync();

        return await _context.Activities
            .Where(a => a.EventId == eventId && !inscritas.Contains(a.Id))
            .ToListAsync();
    }
}