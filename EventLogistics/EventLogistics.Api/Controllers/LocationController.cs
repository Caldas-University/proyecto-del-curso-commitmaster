using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<Location>>> GetAll()
        {
            var locations = await _locationRepository.GetAllAsync();
            return Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetById(int id)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null)
                return NotFound();
            return Ok(location);
        }

        [HttpPost]
        public async Task<ActionResult<Location>> Create(Location location)
        {
            location.CreatedBy = location.CreatedBy ?? "System";
            location.UpdatedBy = location.UpdatedBy ?? "System";
            var result = await _locationRepository.AddAsync(location);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Location location)
        {
            if (id != location.Id)
                return BadRequest();

            location.UpdatedBy = location.UpdatedBy ?? "System";
            await _locationRepository.UpdateAsync(location);
            return NoContent();
        }

        [HttpPut("{id}/availability")]
        public async Task<IActionResult> ChangeAvailability(int id, [FromQuery] bool available)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null)
                return NotFound();

            location.State = available;
            location.UpdatedBy = "System";
            location.UpdatedAt = DateTime.UtcNow;

            await _locationRepository.UpdateAsync(location);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _locationRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}