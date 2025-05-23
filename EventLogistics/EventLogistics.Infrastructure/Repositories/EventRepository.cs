using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
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