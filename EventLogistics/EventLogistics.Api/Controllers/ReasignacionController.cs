namespace EventLogistics.Api.Controllers;

using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ReasignacionController : ControllerBase
{
    private readonly IReasignacionServiceApp _reasignacionService;

    public ReasignacionController(IReasignacionServiceApp reasignacionService)
    {
        _reasignacionService = reasignacionService;
    }

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
}
