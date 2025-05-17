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

        public ReasignacionController(IReassignmentRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
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

            await _ruleRepository.UpdateAsync(rule);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ruleRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}