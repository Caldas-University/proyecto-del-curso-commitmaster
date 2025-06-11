using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventLogistics.Infrastructure.Repositories;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly EventLogisticsDbContext _context;

    public AttendanceRepository(EventLogisticsDbContext context)
    {
        _context = context;
    }

    public async Task<Attendance?> GetByIdAsync(Guid id)
    {
        return await _context.Attendances.FindAsync(id);
    }

    public async Task<List<Attendance>> GetByParticipantAsync(Guid participantId, Guid eventId)
    {
        return await _context.Attendances
            .Where(a => a.ParticipantId == participantId && a.EventId == eventId)
            .ToListAsync();
    }

    public async Task AddAsync(Attendance attendance)
    {
        await _context.Attendances.AddAsync(attendance);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid participantId, Guid eventId)
    {
        return await _context.Attendances
            .AnyAsync(a => a.ParticipantId == participantId && a.EventId == eventId);
    }
}