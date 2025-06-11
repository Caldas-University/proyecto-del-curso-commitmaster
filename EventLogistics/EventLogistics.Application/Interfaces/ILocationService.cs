using EventLogistics.Domain.Entities;

namespace EventLogistics.Application.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<Location?> GetLocationByIdAsync(Guid id);
        Task<IEnumerable<Location>> GetAvailableLocationsAsync();
        Task<IEnumerable<Location>> GetLocationsByStatusAsync(string status);
        Task<Location> CreateLocationAsync(string name, string address, string status = "Disponible");
        Task<Location> UpdateLocationAsync(Guid id, string? name = null, string? address = null, string? status = null);
        Task DeleteLocationAsync(Guid id);
        Task<bool> IsLocationAvailableAsync(Guid locationId);
    }
}
