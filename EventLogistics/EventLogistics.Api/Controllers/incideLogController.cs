using Microsoft.AspNetCore.Mvc;

using EventLogistics.Application.Contracts.Services;
using EventLogistics.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentLogController : ControllerBase
    {
        private readonly IIncidentServiceApp _incidentService;

        public IncidentLogController(IIncidentServiceApp incidentService)
        {
            _incidentService = incidentService;
        }

        // GET: api/IncidentLog/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Incident>> GetIncident(Guid id)
        {
            var incident = await _incidentService.GetIncidentByIdAsync(id);
            if (incident == null)
            {
                return NotFound();
            }

            return Ok(incident);
        }

        // GET: api/IncidentLog/event/{eventId}
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<Incident>>> GetIncidentsByEvent(Guid eventId)
        {
            var incidents = await _incidentService.GetIncidentsByEventIdAsync(eventId);
            return Ok(incidents);
        }

        // POST: api/IncidentLog
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateIncident([FromBody] Incident incident)
        {
            var id = await _incidentService.CreateIncidentAsync(
                incident.EventId,
                incident.Description,
                incident.Location,
                incident.IncidentDate
            );

            return CreatedAtAction(nameof(GetIncident), new { id = id }, id);
        }

        // PUT: api/IncidentLog/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncident(Guid id, [FromBody] Incident incident)
        {
            if (id != incident.Id)
                return BadRequest();

            await _incidentService.UpdateIncidentAsync(id, incident.Description, incident.Location, incident.IncidentDate);
            return NoContent();
        }

        // DELETE: api/IncidentLog/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(Guid id)
        {
            await _incidentService.DeleteIncidentAsync(id);
            return NoContent();
        }
    }
}
