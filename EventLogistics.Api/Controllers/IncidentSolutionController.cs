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
    public class IncidentSolutionController : ControllerBase
    {
        private readonly IIncidentServiceApp _incidentService;

        public IncidentSolutionController(IIncidentServiceApp incidentService)
        {
            _incidentService = incidentService;
        }        // GET: api/IncidentSolution/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<IncidentSolution>> GetIncidentSolution(Guid id)
        {
            var solution = await _incidentService.GetIncidentSolutionByIdAsync(id);
            if (solution == null)
            {
                return NotFound();
            }
            return Ok(solution);
        }

        // GET: api/IncidentSolution/incident/{incidentId}
        [HttpGet("incident/{incidentId}")]
        public async Task<ActionResult<IEnumerable<IncidentSolution>>> GetSolutionsByIncident(Guid incidentId)
        {
            var solutions = await _incidentService.GetSolutionsByIncidentIdAsync(incidentId);
            return Ok(solutions);
        }

        // POST: api/IncidentSolution
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateIncidentSolution([FromBody] CreateIncidentSolutionRequest request)
        {
            var solutionId = await _incidentService.ApplyIncidentSolutionAsync(
                request.IncidentId, 
                request.ActionTaken, 
                request.AppliedBy);
            return CreatedAtAction(nameof(GetIncidentSolution), new { id = solutionId }, solutionId);
        }

        // PUT: api/IncidentSolution/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncidentSolution(Guid id, [FromBody] IncidentSolution solution)
        {
            try
            {
                await _incidentService.UpdateIncidentSolutionAsync(id, solution);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        // DELETE: api/IncidentSolution/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncidentSolution(Guid id)
        {
            await _incidentService.DeleteIncidentSolutionAsync(id);
            return NoContent();
        }
    }

    // Request DTOs
    public class CreateIncidentSolutionRequest
    {
        public Guid IncidentId { get; set; }
        public string ActionTaken { get; set; } = string.Empty;
        public string AppliedBy { get; set; } = string.Empty;
    }
}