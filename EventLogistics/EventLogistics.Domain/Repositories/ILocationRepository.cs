using EventLogistics.Domain.Entities;

namespace EventLogistics.Domain.Repositories
{
    public interface ILocationRepository : IRepository<Location>
    {
        Task<IEnumerable<Location>> GetAvailableLocationsAsync();
        Task<IEnumerable<Location>> GetLocationsByStatusAsync(string status);
        Task<Location?> GetByNameAsync(string name);
    }
}
