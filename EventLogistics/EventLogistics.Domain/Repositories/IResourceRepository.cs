namespace EventLogistics.Domain.Repositories;

using EventLogistics.Domain.Entities;

public interface IResourceRepository
{
    Task<Resource> GetByIdAsync(Guid resourceId);
    Task UpdateAsync(Resource resource);
    Task<bool> AssignResourceAsync(Guid resourceId, Guid eventId);
    Task AddAsync(Resource resource);
    Task<List<Resource>> GetAvailableResourcesAsync();
}