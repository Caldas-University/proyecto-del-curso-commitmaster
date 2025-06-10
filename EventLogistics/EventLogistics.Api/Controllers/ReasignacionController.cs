using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using EventLogistics.Application.Services;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReasignacionController : ControllerBase
    {
        private readonly IReasignacionServiceApp _reasignacionService;
        private readonly IReassignmentRuleRepository _ruleRepository;
        private readonly ReassignmentService _reassignmentService;

        public ReasignacionController(
            IReasignacionServiceApp reasignacionService,
            IReassignmentRuleRepository ruleRepository,
            ReassignmentService reassignmentService)
        {
            _reasignacionService = reasignacionService;
            _ruleRepository = ruleRepository;
            _reassignmentService = reassignmentService;
        }

        // Métodos de la rama HEAD (camilo)
        [HttpPost]
        public async Task<ActionResult<ReasignacionDto>> CreateReasignacion([FromBody] CreateReasignacionRequest request)
        {
            var reasignacion = await _reasignacionService.CreateReasignacionAsync(request.Tipo);
            return Ok(reasignacion);
        }

        [HttpPost("search-event")]
        public async Task<ActionResult<List<Guid>>> SearchEvent([FromBody] Dictionary<string, object> criterios)
        {
            var events = await _reasignacionService.SearchEventAsync(criterios);
            return Ok(events);
        }

        [HttpPost("detect-change")]
        public async Task<ActionResult<bool>> DetectChange()
        {
            var result = await _reasignacionService.DetectChangeAsync();
            return Ok(result);
        }

        [HttpPost("analyze-rules")]
        public async Task<ActionResult<Dictionary<string, object>>> AnalyzeRules()
        {
            var rules = await _reasignacionService.AnalyzeRulesAsync();
            return Ok(rules);
        }

        [HttpPost("reassign-resources")]
        public async Task<ActionResult<bool>> ReasignResources([FromBody] ReasignResourcesRequest request)
        {
            var result = await _reasignacionService.ReasignResourcesAsync(request.Recursos, request.Eventos);
            return Ok(result);
        }

        [HttpPost("reschedule-activities")]
        public async Task<ActionResult<bool>> RescheduleActivities([FromBody] RescheduleActivitiesRequest request)
        {
            var result = await _reasignacionService.RescheduleActivitiesAsync(request.Actividades);
            return Ok(result);
        }

        [HttpGet("evaluate-impact")]
        public async Task<ActionResult<Dictionary<string, double>>> EvaluateImpact()
        {
            var impact = await _reasignacionService.EvaluateImpactAsync();
            return Ok(impact);
        }

        [HttpPost("manage-conflicts")]
        public async Task<ActionResult<bool>> ManageConflicts([FromBody] List<Dictionary<string, object>> conflictos)
        {
            var result = await _reasignacionService.ManageConflictsAsync(conflictos);
            return Ok(result);
        }

        // Métodos de origin/dev
        [HttpGet("rules")]
        public async Task<ActionResult<IEnumerable<ReassignmentRule>>> GetAllRules()
        {
            var rules = await _ruleRepository.GetAllAsync();
            return Ok(rules);
        }

        [HttpGet("rules/active")]
        public async Task<ActionResult<IEnumerable<ReassignmentRule>>> GetActiveRules()
        {
            var rules = await _ruleRepository.GetActiveRulesAsync();
            return Ok(rules);
        }

        [HttpGet("rules/{id}")]
        public async Task<ActionResult<ReassignmentRule>> GetRuleById(int id)
        {
            var rule = await _ruleRepository.GetByIdAsync(id);
            if (rule == null)
            {
                return NotFound();
            }
            return Ok(rule);
        }

        [HttpPost("rules")]
        public async Task<ActionResult<ReassignmentRule>> CreateRule(ReassignmentRule rule)
        {
            // Establecer valores predeterminados
            rule.CreatedBy = rule.CreatedBy ?? "System";
            rule.UpdatedBy = rule.UpdatedBy ?? "System";

            var result = await _ruleRepository.AddAsync(rule);
            return CreatedAtAction(nameof(GetRuleById), new { id = result.Id }, result);
        }

        [HttpPut("rules/{id}")]
        public async Task<IActionResult> UpdateRule(int id, ReassignmentRule rule)
        {
            if (id != rule.Id)
            {
                return BadRequest();
            }

            rule.UpdatedBy = rule.UpdatedBy ?? "System";
            await _ruleRepository.UpdateAsync(rule);
            return NoContent();
        }

        [HttpDelete("rules/{id}")]
        public async Task<IActionResult> DeleteRule(int id)
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

        [HttpGet("evaluate-impact/{eventId}/{resourceId}")]
        public async Task<IActionResult> EvaluateImpactSpecific(int eventId, int resourceId)
        {
            var impact = _reassignmentService.EvaluateImpact(eventId, resourceId);
            return Ok(impact);
        }

        [HttpPut("modify/{id}")]
        public async Task<IActionResult> ModifyAssignment(
            int id, 
            [FromBody] ModifyAssignmentRequest request)
        {
            var result = await _reassignmentService.ModifyAssignment(
                id, 
                request.NewQuantity, 
                request.NewStartTime
            );

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("auto-reassign/{id}")]
        public async Task<IActionResult> ReassignAutomatically(
            int id, 
            [FromQuery] string reason)
        {
            var result = await _reassignmentService.ReassignAutomatically(id, reason);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // DTOs
        public class CreateReasignacionRequest
        {
            public string Tipo { get; set; } = string.Empty;
        }

        public class ReasignResourcesRequest
        {
            public List<Guid> Recursos { get; set; } = new();
            public List<Guid> Eventos { get; set; } = new();
        }

        public class RescheduleActivitiesRequest
        {
            public List<Guid> Actividades { get; set; } = new();
        }

        public class ModifyAssignmentRequest
        {
            public int NewQuantity { get; set; }
            public DateTime? NewStartTime { get; set; }
        }
    }
}
