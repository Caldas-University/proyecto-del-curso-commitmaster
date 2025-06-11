using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;

        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetAllLocations()
        {
            try
            {
                var locations = await _locationRepository.GetAllAsync();
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(Guid id)
        {
            try
            {
                var location = await _locationRepository.GetByIdAsync(id);
                if (location == null)
                    return NotFound();

                return Ok(location);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Location>>> GetAvailableLocations()
        {
            try
            {
                var locations = await _locationRepository.GetAvailableLocationsAsync();
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationsByStatus(string status)
        {
            try
            {
                var validStatuses = new[] { "Disponible", "Ocupado", "Mantenimiento" };
                if (!validStatuses.Contains(status))
                    return BadRequest("Estado no válido. Los estados válidos son: Disponible, Ocupado, Mantenimiento");

                var locations = await _locationRepository.GetLocationsByStatusAsync(status);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Location>> CreateLocation(CreateLocationRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    return BadRequest("El nombre es requerido");

                if (string.IsNullOrWhiteSpace(request.Address))
                    return BadRequest("La dirección es requerida");

                var validStatuses = new[] { "Disponible", "Ocupado", "Mantenimiento" };
                if (!string.IsNullOrEmpty(request.Status) && !validStatuses.Contains(request.Status))
                    return BadRequest("Estado no válido. Los estados válidos son: Disponible, Ocupado, Mantenimiento");

                var existingLocation = await _locationRepository.GetByNameAsync(request.Name);
                if (existingLocation != null)
                    return BadRequest("Ya existe una ubicación con ese nombre");

                var location = new Location(request.Name, request.Address, request.Status ?? "Disponible");
                var createdLocation = await _locationRepository.AddAsync(location);

                return CreatedAtAction(nameof(GetLocation), new { id = createdLocation.Id }, createdLocation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Location>> UpdateLocation(Guid id, UpdateLocationRequest request)
        {
            try
            {
                var location = await _locationRepository.GetByIdAsync(id);
                if (location == null)
                    return NotFound();

                if (!string.IsNullOrWhiteSpace(request.Name))
                    location.Name = request.Name;

                if (!string.IsNullOrWhiteSpace(request.Address))
                    location.Address = request.Address;

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    var validStatuses = new[] { "Disponible", "Ocupado", "Mantenimiento" };
                    if (!validStatuses.Contains(request.Status))
                        return BadRequest("Estado no válido. Los estados válidos son: Disponible, Ocupado, Mantenimiento");

                    location.UpdateStatus(request.Status);
                }

                location.UpdatedAt = DateTime.UtcNow;
                location.UpdatedBy = "User"; // En un sistema real, esto vendría del usuario autenticado

                var updatedLocation = await _locationRepository.UpdateAsync(location);
                return Ok(updatedLocation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLocation(Guid id)
        {
            try
            {
                var location = await _locationRepository.GetByIdAsync(id);
                if (location == null)
                    return NotFound();

                await _locationRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }

    public class CreateLocationRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Status { get; set; }
    }

    public class UpdateLocationRequest
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Status { get; set; }
    }
}
