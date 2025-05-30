using EventLogistics.Application.DTOs;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityController(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var activities = await _activityRepository.GetAllAsync();
            return activities.Any() ? Ok(activities) : NotFound("No hay actividades registradas");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var activity = await _activityRepository.GetByIdAsync(id);
            return activity != null ? Ok(activity) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActivityCreateDto dto)
        {
            if (dto == null)
                return BadRequest();

            var activity = new Activity
            {
                Name = dto.ActivityName, // Corregido
                Location = dto.Location,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                EventId = dto.EventId
            };

            await _activityRepository.AddAsync(activity);
            return CreatedAtAction(nameof(Get), new { id = activity.Id }, activity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActivityUpdateDto dto)
        {
            if (dto == null)
                return BadRequest();

            var existing = await _activityRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = dto.ActivityName; // Corregido
            existing.Location = dto.Location;
            existing.StartTime = dto.StartTime;
            existing.EndTime = dto.EndTime;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedBy = "System";

            await _activityRepository.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var activity = await _activityRepository.GetByIdAsync(id);
            if (activity == null)
                return NotFound();

            await _activityRepository.DeleteAsync(activity.Id);
            return NoContent();
        }
    }
}