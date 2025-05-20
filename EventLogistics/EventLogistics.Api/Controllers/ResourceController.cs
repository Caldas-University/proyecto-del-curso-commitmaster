using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly IRepository<Resource> _resourceRepository;

        public ResourceController(IRepository<Resource> resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resource>>> GetAll()
        {
            var resources = await _resourceRepository.GetAllAsync();
            return Ok(resources);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> GetById(int id)
        {
            var resource = await _resourceRepository.GetByIdAsync(id);
            if (resource == null)
            {
                return NotFound();
            }
            return Ok(resource);
        }

        [HttpPost]
        public async Task<ActionResult<Resource>> Create(Resource resource)
        {
            resource.CreatedBy = resource.CreatedBy ?? "System";
            resource.UpdatedBy = resource.UpdatedBy ?? "System";
            
            var result = await _resourceRepository.AddAsync(resource);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Resource resource)
        {
            if (id != resource.Id)
            {
                return BadRequest();
            }

            resource.UpdatedBy = resource.UpdatedBy ?? "System";
            await _resourceRepository.UpdateAsync(resource);
            return NoContent();
        }

        [HttpPut("{id}/availability")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromBody] bool availability)
        {
            var resource = await _resourceRepository.GetByIdAsync(id);
            if (resource == null)
            {
                return NotFound();
            }
            
            resource.Availability = availability;
            resource.UpdatedBy = "System";
            await _resourceRepository.UpdateAsync(resource);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _resourceRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}