<<<<<<< HEAD
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventLogistics.Infrastructure.Repositories;

public class ResourceRepository : IResourceRepository
{
    private readonly EventLogisticsDbContext _context;

    public ResourceRepository(EventLogisticsDbContext context)
    {
        _context = context;
    }

    public async Task<Resource?> GetByIdAsync(Guid id)
    {
        return await _context.Resources.FindAsync(id);
    }

    public async Task<IEnumerable<Resource>> GetAllAsync()
    {
        return await _context.Resources.ToListAsync();
    }

    public async Task<IEnumerable<Resource>> GetAvailableResourcesAsync()
    {
        return await _context.Resources.Where(r => r.Availability).ToListAsync();
    }

    public async Task<Resource> AddAsync(Resource resource)
    {
        _context.Resources.Add(resource);
        await _context.SaveChangesAsync();
        return resource;
    }

    public async Task UpdateAsync(Resource resource)
    {
        _context.Resources.Update(resource);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var resource = await GetByIdAsync(id);
        if (resource != null)
        {
            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> AssignResourceAsync(Guid resourceId, Guid eventId)
    {
        var resource = await GetByIdAsync(resourceId);
        if (resource != null && resource.Availability)
        {
            resource.Assignments.Add(eventId);
            await UpdateAsync(resource);
            return true;
        }
        return false;
=======
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
        public async Task<bool> ValidateResourceAvailability(int resourceId, DateTime start, DateTime end)
        {
            return await _context.Resources
                .Where(r => r.Id == resourceId)
                .AnyAsync(r => r.Availability && 
                              !r.Assignments.Any(a => 
                                  a.Status != "Cancelled" &&
                                  a.StartTime < end && 
                                  a.EndTime > start));
        }
>>>>>>> sebas
    }
}