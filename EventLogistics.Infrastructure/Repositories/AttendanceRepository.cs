using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventLogistics.Infrastructure.Repositories;

/// <summary>
/// Implementación de IAttendanceRepository usando Entity Framework Core.
/// </summary>
public class AttendanceRepository : IAttendanceRepository
{
    private readonly EventLogisticsDbContext _context;

    public AttendanceRepository(EventLogisticsDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<Attendance?> GetByIdAsync(Guid id)
    {
        return await _context.Attendances.FindAsync(id);
    }

    /// <inheritdoc/>
    public async Task<List<Attendance>> GetByParticipantAsync(Guid participantId)
    {
        return await _context.Attendances
            .Where(a => a.ParticipantId == participantId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<Attendance>> GetByParticipantAsync(Guid participantId, Guid eventId)
    {
        return await _context.Attendances
            .Where(a => a.ParticipantId == participantId && a.EventId == eventId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task AddAsync(Attendance attendance)
    {
        await _context.Attendances.AddAsync(attendance);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(Guid participantId, Guid eventId)
    {
        return await _context.Attendances
            .AnyAsync(a => a.ParticipantId == participantId && a.EventId == eventId);
    }

    /// <inheritdoc/>
    public async Task<Attendance?> GetByParticipantAndEventAsync(Guid participantId, Guid eventId)
    {
        return await _context.Attendances
            .FirstOrDefaultAsync(a => a.ParticipantId == participantId && a.EventId == eventId);
    }

    /// <inheritdoc/>
    public async Task<List<Attendance>> GetAllByEventAsync(Guid eventId)
    {
        return await _context.Attendances
            .Where(a => a.EventId == eventId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<Attendance>> GetAllAsync()
    {
        return await _context.Attendances.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id)
    {
        var attendance = await _context.Attendances.FindAsync(id);
        if (attendance != null)
        {
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
        }
    }
}