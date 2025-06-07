using EventLogistics.Application.Services;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReasignacionController : ControllerBase
    {
        private readonly IReassignmentRuleRepository _ruleRepository;
        private readonly ReassignmentService _reassignmentService;

        public ReasignacionController(
            IReassignmentRuleRepository ruleRepository,
            ReassignmentService reassignmentService)
        {
            _ruleRepository = ruleRepository;
            _reassignmentService = reassignmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReassignmentRule>>> GetAll()
        {
            var rules = await _ruleRepository.GetAllAsync();
            return Ok(rules);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ReassignmentRule>>> GetActiveRules()
        {
            var rules = await _ruleRepository.GetActiveRulesAsync();
            return Ok(rules);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReassignmentRule>> GetById(int id)
        {
            var rule = await _ruleRepository.GetByIdAsync(id);
            if (rule == null)
            {
                return NotFound();
            }
            return Ok(rule);
        }

        [HttpPost]
        public async Task<ActionResult<ReassignmentRule>> Create(ReassignmentRule rule)
        {
            // Establecer valores predeterminados
            rule.CreatedBy = rule.CreatedBy ?? "System";
            rule.UpdatedBy = rule.UpdatedBy ?? "System";
            
            var result = await _ruleRepository.AddAsync(rule);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ReassignmentRule rule)
        {
            if (id != rule.Id)
            {
                return BadRequest();
            }

            rule.UpdatedBy = rule.UpdatedBy ?? "System";
            await _ruleRepository.UpdateAsync(rule);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ruleRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("process-change")]
        public async Task<IActionResult> ProcessResourceChange(int resourceId, bool newAvailability)
        {
            var result = await _reassignmentService.ProcessResourceChange(resourceId, newAvailability);
            if (result)
            {
                return Ok(new { message = "Resource change processed successfully" });
            }
            return BadRequest(new { message = "Failed to process resource change" });
        }

        [HttpGet("evaluate-impact")]
        public async Task<IActionResult> EvaluateImpact(int eventId, int resourceId)
        {
            var impact = _reassignmentService.EvaluateImpact(eventId, resourceId);
            return Ok(impact);
        }
    }
}