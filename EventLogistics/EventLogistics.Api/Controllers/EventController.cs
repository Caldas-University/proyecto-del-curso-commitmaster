using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetAll()
        {
            var events = await _eventRepository.GetAllAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetById(int id)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return Ok(eventItem);
        }
        [HttpPost]
        public async Task<ActionResult<Event>> Create(Event eventItem)
        {
            // Establecer valores predeterminados
            eventItem.CreatedBy = eventItem.CreatedBy ?? "System";
            eventItem.UpdatedBy = eventItem.UpdatedBy ?? "System";

            var result = await _eventRepository.AddAsync(eventItem);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Event eventItem)
        {
            if (id != eventItem.Id)
            {
                return BadRequest();
            }

            eventItem.UpdatedBy = eventItem.UpdatedBy ?? "System";
            var result = await _eventRepository.UpdateAsync(eventItem);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _eventRepository.GetByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            await _eventRepository.DeleteAsync(eventItem);
            return NoContent();
        }

    }
}