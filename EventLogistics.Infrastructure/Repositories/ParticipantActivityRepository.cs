using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventLogistics.Infrastructure.Repositories;

public class ParticipantActivityRepository : IParticipantActivityRepository
{
    private readonly EventLogisticsDbContext _context;

    public ParticipantActivityRepository(EventLogisticsDbContext context)
    {
        _context = context;
    }

    // Mejor usar Expression<Func<...>> para consultas asíncronas y eficientes en EF Core
    public async Task<bool> AnyAsync(Expression<Func<ParticipantActivity, bool>> predicate)
        => await _context.ParticipantActivities.Include(pa => pa.Activity).AnyAsync(predicate);

    public async Task<List<ParticipantActivity>> GetAllAsync()
    {
        return await _context.ParticipantActivities
            .Include(pa => pa.Activity)
            .ToListAsync();
    }

    public async Task AddAsync(ParticipantActivity participantActivity)
    {
        await _context.ParticipantActivities.AddAsync(participantActivity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid participantId, Guid activityId)
    {
        var entity = await _context.ParticipantActivities
            .FirstOrDefaultAsync(pa => pa.ParticipantId == participantId && pa.ActivityId == activityId);
        if (entity != null)
        {
            _context.ParticipantActivities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ParticipantActivity?> GetByParticipantAndActivityAsync(Guid participantId, Guid activityId)
    {
        return await _context.ParticipantActivities
            .Include(pa => pa.Activity)
            .FirstOrDefaultAsync(pa => pa.ParticipantId == participantId && pa.ActivityId == activityId);
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