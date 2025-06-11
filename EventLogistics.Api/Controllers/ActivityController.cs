using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> GetAll()
        {
            var activities = await _activityService.GetAllAsync();
            return Ok(activities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetById(Guid id)
        {
            var activity = await _activityService.GetByIdAsync(id);
            if (activity == null)
                return NotFound();
            
            return Ok(activity);
        }        [HttpPost]
        public async Task<ActionResult<ActivityDto>> Create([FromBody] CreateActivityRequest activityRequest)
        {
            try
            {
                var result = await _activityService.CreateAsync(activityRequest);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al crear actividad", details = ex.Message });
            }
        }
    }
}
