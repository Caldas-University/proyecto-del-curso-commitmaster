using EventLogistics.Domain.Entities;

namespace EventLogistics.Domain.Repositories;

public interface IResourceRepository
{
    Task<Resource?> GetByIdAsync(Guid id);
    Task<IEnumerable<Resource>> GetAllAsync();
    Task<IEnumerable<Resource>> GetAvailableResourcesAsync();
    Task<Resource> AddAsync(Resource resource);
    Task UpdateAsync(Resource resource);
    Task DeleteAsync(Guid id);
    Task<bool> AssignResourceAsync(Guid resourceId, Guid eventId);
}