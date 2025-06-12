using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventLogistics.Infrastructure.Repositories;

/// <summary>
/// Implementación de IParticipantRepository usando Entity Framework Core.
/// </summary>
public class ParticipantRepository : IParticipantRepository
{
    private readonly EventLogisticsDbContext _context;

    public ParticipantRepository(EventLogisticsDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<Participant?> GetByIdAsync(Guid id)
    {
        return await _context.Participants.FindAsync(id);
    }

    /// <inheritdoc/>
    public async Task<Participant?> GetByDocumentAsync(string document)
    {
        return await _context.Participants.FirstOrDefaultAsync(p => p.Document == document);
    }

    /// <inheritdoc/>
    public async Task<Participant?> GetByEmailAsync(string email)
    {
        return await _context.Participants.FirstOrDefaultAsync(p => p.Email == email);
    }

    /// <inheritdoc/>
    public async Task AddAsync(Participant participant)
    {
        await _context.Participants.AddAsync(participant);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<List<Participant>> GetAllAsync()
    {
        return await _context.Participants.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Participant participant)
    {
        _context.Participants.Update(participant);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id)
    {
        var participant = await _context.Participants.FindAsync(id);
        if (participant != null)
        {
            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
        }
    }
}