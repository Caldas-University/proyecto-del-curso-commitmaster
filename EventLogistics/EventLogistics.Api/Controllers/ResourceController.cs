namespace EventLogistics.Api.Controllers;

using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ResourceController : ControllerBase
{
    private readonly IResourceServiceApp _resourceService;

    public ResourceController(IResourceServiceApp resourceService)
    {
        _resourceService = resourceService;
    }

    [HttpGet("{resourceId}")]
    public async Task<ActionResult<ResourceDto>> CheckAvailability(Guid resourceId)
    {
        var resource = await _resourceService.CheckAvailabilityAsync(resourceId);
        if (resource == null)
        {
            return NotFound();
        }
        return Ok(resource);
    }

    [HttpPut("{resourceId}/status")]
    public async Task<ActionResult<ResourceDto>> UpdateResourceStatus(Guid resourceId, [FromBody] string status)
    {
        var updatedResource = await _resourceService.UpdateResourceStatusAsync(resourceId, status);
        if (updatedResource == null)
        {
            return NotFound();
        }
        return Ok(updatedResource);
    }

    [HttpPost("{resourceId}/assign/{eventId}")]
    public async Task<ActionResult<bool>> AssignResource(Guid resourceId, Guid eventId)
    {
        var result = await _resourceService.AssignResourceAsync(resourceId, eventId);
        if (!result)
        {
            return BadRequest("Failed to assign resource.");
        }
        return Ok(result);
    }
}