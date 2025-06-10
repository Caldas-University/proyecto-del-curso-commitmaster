namespace EventLogistics.Infrastructure.Repositories;

using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class EventRepository : IEventRepository
{
    private readonly EventLogisticsDbContext _dbContext;

    public EventRepository(EventLogisticsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Event?> GetEventAsync(Guid id)
    {
        return await _dbContext.Events.FindAsync(id);
    }

    public async Task<List<Event>> GetAllAsync()
    {
        return await _dbContext.Events.ToListAsync();
    }

    public async Task<Event?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Events.FindAsync(id);
    }
}
