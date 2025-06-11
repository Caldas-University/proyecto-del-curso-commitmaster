using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using System.Linq;

namespace EventLogistics.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _locationRepository.GetAllAsync();
        }

        public async Task<Location?> GetLocationByIdAsync(Guid id)
        {
            return await _locationRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Location>> GetAvailableLocationsAsync()
        {
            return await _locationRepository.GetAvailableLocationsAsync();
        }

        public async Task<IEnumerable<Location>> GetLocationsByStatusAsync(string status)
        {
            var validStatuses = new[] { "Disponible", "Ocupado", "Mantenimiento" };
            if (!validStatuses.Contains(status))
                throw new ArgumentException("Estado no válido. Los estados válidos son: Disponible, Ocupado, Mantenimiento", nameof(status));

            return await _locationRepository.GetLocationsByStatusAsync(status);
        }

        public async Task<Location> CreateLocationAsync(string name, string address, string status = "Disponible")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre es requerido", nameof(name));

            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("La dirección es requerida", nameof(address));

            var validStatuses = new[] { "Disponible", "Ocupado", "Mantenimiento" };
            if (!validStatuses.Contains(status))
                throw new ArgumentException("Estado no válido. Los estados válidos son: Disponible, Ocupado, Mantenimiento", nameof(status));

            // Verificar si ya existe una ubicación con el mismo nombre
            var existingLocation = await _locationRepository.GetByNameAsync(name);
            if (existingLocation != null)
                throw new InvalidOperationException("Ya existe una ubicación con ese nombre");

            var location = new Location(name, address, status);
            return await _locationRepository.AddAsync(location);
        }

        public async Task<Location> UpdateLocationAsync(Guid id, string? name = null, string? address = null, string? status = null)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null)
                throw new InvalidOperationException("Ubicación no encontrada");

            if (!string.IsNullOrWhiteSpace(name))
                location.Name = name;

            if (!string.IsNullOrWhiteSpace(address))
                location.Address = address;

            if (!string.IsNullOrWhiteSpace(status))
            {
                var validStatuses = new[] { "Disponible", "Ocupado", "Mantenimiento" };
                if (!validStatuses.Contains(status))
                    throw new ArgumentException("Estado no válido. Los estados válidos son: Disponible, Ocupado, Mantenimiento", nameof(status));

                location.UpdateStatus(status);
            }

            location.UpdatedAt = DateTime.UtcNow;
            location.UpdatedBy = "System"; // En un sistema real, esto vendría del usuario autenticado

            return await _locationRepository.UpdateAsync(location);
        }

        public async Task DeleteLocationAsync(Guid id)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null)
                throw new InvalidOperationException("Ubicación no encontrada");

            await _locationRepository.DeleteAsync(id);
        }

        public async Task<bool> IsLocationAvailableAsync(Guid locationId)
        {
            var location = await _locationRepository.GetByIdAsync(locationId);
            return location?.IsAvailable() ?? false;
        }
    }
}
