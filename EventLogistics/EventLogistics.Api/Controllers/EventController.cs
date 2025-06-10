using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EventDto>>> GetAll()
        {
            var events = await _eventService.GetAllAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetById(Guid id)
        {
            var eventDto = await _eventService.GetEventByIdAsync(id);
            if (eventDto == null)
            {
                return NotFound();
            }
            return Ok(eventDto);
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> Create(EventDto eventDto)
        {
            try
            {
                var result = await _eventService.CreateAsync(eventDto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al crear evento", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, EventDto eventDto)
        {
            if (id != eventDto.Id)
            {
                return BadRequest("El ID del evento no coincide");
            }

            try
            {
                var result = await _eventService.UpdateAsync(eventDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al actualizar evento", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _eventService.DeleteAsync(id);
                if (success)
                {
                    return NoContent();
                }
                return NotFound("Evento no encontrado");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al eliminar evento", details = ex.Message });
            }
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<EventDto>>> GetByStatus(string status)
        {
            var events = await _eventService.GetEventsByStatusAsync(status);
            return Ok(events);
        }

        [HttpGet("place/{place}")]
        public async Task<ActionResult<List<EventDto>>> GetByPlace(string place)
        {
            var events = await _eventService.GetEventsByPlaceAsync(place);
            return Ok(events);
        }

        [HttpGet("upcoming")]
        public async Task<ActionResult<List<EventDto>>> GetUpcomingEvents()
        {
            var allEvents = await _eventService.GetAllAsync();
            var upcomingEvents = allEvents
                .Where(e => e.Schedule > DateTime.UtcNow)
                .OrderBy(e => e.Schedule)
                .ToList();
            
            return Ok(upcomingEvents);
        }

        [HttpGet("today")]
        public async Task<ActionResult<List<EventDto>>> GetTodayEvents()
        {
            var allEvents = await _eventService.GetAllAsync();
            var today = DateTime.Today;
            var todayEvents = allEvents
                .Where(e => e.Schedule.Date == today)
                .OrderBy(e => e.Schedule)
                .ToList();
            
            return Ok(todayEvents);
        }
    }
}