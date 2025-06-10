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
    }
}