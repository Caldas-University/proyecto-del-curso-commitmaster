using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILocationRepository _locationRepository;

        public EventsController(IEventRepository eventRepository, ILocationRepository locationRepository)
        {
            _eventRepository = eventRepository;
            _locationRepository = locationRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Event>> Create(Event newEvent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar que la ubicación exista
            var location = await _locationRepository.GetByIdAsync(newEvent.LocationId);
            if (location == null)
                return BadRequest("La ubicación seleccionada no existe.");

            // Validar que la ubicación esté disponible
            if (!location.State)
                return Conflict("La ubicación seleccionada no está disponible.");

            // Validar que EndTime sea mayor que StartTime
            if (newEvent.EndTime <= newEvent.StartTime)
                return BadRequest("La fecha y hora de fin debe ser mayor que la de inicio.");

            // Validar que no haya solapamiento de eventos en la misma ubicación
            var existingEvents = await _eventRepository.GetEventsByLocationAsync(newEvent.LocationId);
            bool overlap = existingEvents.Any(e =>
                (newEvent.StartTime < e.EndTime && newEvent.EndTime > e.StartTime)
            );
            if (overlap)
                return Conflict("Ya existe un evento programado en esa ubicación y horario.");

            var result = await _eventRepository.AddAsync(newEvent);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetById(int id)
        {
            var ev = await _eventRepository.GetEventWithDetailsAsync(id);
            if (ev == null)
                return NotFound();
            return Ok(ev);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetAll()
        {
            var events = await _eventRepository.GetAllAsync();
            return Ok(events);
        }
    }
}