using Microsoft.AspNetCore.Mvc;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentSolutionController : ControllerBase
    {
        private readonly IIncidentSolutionRepository _incidentSolutionRepository;

        public IncidentSolutionController(IIncidentSolutionRepository incidentSolutionRepository)
        {
            _incidentSolutionRepository = incidentSolutionRepository;
        }

        // GET: api/IncidentSolution/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<IncidentSolution>> GetIncidentSolution(Guid id)
        {
            var solution = await _incidentSolutionRepository.GetByIdAsync(id);
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
            var solutions = await _incidentSolutionRepository.GetByIncidentIdAsync(incidentId);
            return Ok(solutions);
        }

        // POST: api/IncidentSolution
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateIncidentSolution([FromBody] IncidentSolution solution)
        {
            solution.Id = Guid.NewGuid();
            solution.DateApplied = DateTime.UtcNow;
            await _incidentSolutionRepository.AddAsync(solution);
            return CreatedAtAction(nameof(GetIncidentSolution), new { id = solution.Id }, solution.Id);
        }

        // PUT: api/IncidentSolution/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncidentSolution(Guid id, [FromBody] IncidentSolution solution)
        {
            if (id != solution.Id)
            {
                return BadRequest();
            }
            await _incidentSolutionRepository.UpdateAsync(solution);
            return NoContent();
        }

        // DELETE: api/IncidentSolution/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncidentSolution(Guid id)
        {
            await _incidentSolutionRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}