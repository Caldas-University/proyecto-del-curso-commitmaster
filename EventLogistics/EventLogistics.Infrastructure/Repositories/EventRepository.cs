<<<<<<< HEAD
=======
namespace EventLogistics.Infrastructure.Repositories;

>>>>>>> d2804b8fb33a385d35d1dddc2534720d8e145fe9
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventLogistics.Infrastructure.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Event>> GetEventsByLocationAsync(int locationId)
        {
            return await _dbSet
                .Where(e => e.LocationId == locationId)
                .Include(e => e.Location)
                .Include(e => e.Resources)
                .ToListAsync();
        }

        public async Task<Event> GetEventWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(e => e.Location)
                .Include(e => e.Resources)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
=======
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
}
>>>>>>> d2804b8fb33a385d35d1dddc2534720d8e145fe9
