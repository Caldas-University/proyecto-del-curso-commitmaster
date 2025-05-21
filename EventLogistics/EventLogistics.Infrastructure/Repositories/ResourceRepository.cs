using EventLogistics.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using EventLogistics.Infrastructure.Persistence;

namespace EventLogistics.Infrastructure.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ApplicationDbContext _context;

        public ResourceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckAvailabilityAsync(string resourceType, int quantity, DateTime date)
        {
            var resource = await _context.Resources
                .FirstOrDefaultAsync(r => r.Type == resourceType && r.FechaInicio <= date && r.FechaFin >= date);
            
            return resource != null && resource.Capacity >= quantity;
        }

        public async Task<bool> ReserveResourceAsync(int resourceId, int quantity)
        {
            var resource = await _context.Resources.FindAsync(resourceId);
            if (resource == null || resource.Capacity < quantity)
                return false;

            resource.Capacity -= quantity;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}