using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventLogistics.Infrastructure.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(EventLogisticsDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Location>> GetAvailableLocationsAsync()
        {
            return await _dbSet
                .Where(l => l.Status == "Disponible")
                .ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetLocationsByStatusAsync(string status)
        {
            return await _dbSet
                .Where(l => l.Status == status)
                .ToListAsync();
        }

        public async Task<Location?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(l => l.Name == name);
        }
    }
}
