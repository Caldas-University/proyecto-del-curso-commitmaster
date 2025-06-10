using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventLogistics.Infrastructure.Repositories;

public class ParticipantRepository : IParticipantRepository
{
    private readonly EventLogisticsDbContext _context;

    public ParticipantRepository(EventLogisticsDbContext context)
    {
        _context = context;
    }

    public async Task<Participant?> GetByIdAsync(Guid id)
    {
        return await _context.Participants.FindAsync(id);
    }

    public async Task<Participant?> GetByDocumentAsync(string document)
    {
        return await _context.Participants.FirstOrDefaultAsync(p => p.Document == document);
    }

    public async Task<Participant?> GetByEmailAsync(string email)
    {
        return await _context.Participants.FirstOrDefaultAsync(p => p.Email == email);
    }

    public async Task AddAsync(Participant participant)
    {
        await _context.Participants.AddAsync(participant);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Participant>> GetAllAsync()
    {
        return await _context.Participants.ToListAsync();
    }
}