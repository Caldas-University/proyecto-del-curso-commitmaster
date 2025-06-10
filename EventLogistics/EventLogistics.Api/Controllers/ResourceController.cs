<<<<<<< HEAD
namespace EventLogistics.Api.Controllers;

using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
=======
using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
>>>>>>> sebas

[ApiController]
[Route("api/[controller]")]
public class ResourceController : ControllerBase
{
    private readonly IResourceServiceApp _resourceService;

    public ResourceController(IResourceServiceApp resourceService)
    {
<<<<<<< HEAD
        _resourceService = resourceService;
    }

    [HttpGet("{resourceId}")]
    public async Task<ActionResult<ResourceDto>> CheckAvailability(Guid resourceId)
    {
        var resource = await _resourceService.CheckAvailabilityAsync(resourceId);
        if (resource == null)
        {
            return NotFound();
=======
        private readonly IRepository<Resource> _resourceRepository;
        private readonly IEmailService _emailService;
        private readonly IReassignmentService _reassignmentService;
        public ResourceController(
            IRepository<Resource> resourceRepository,
            IEmailService emailService,
            IReassignmentService reassignmentService)
        {
            _resourceRepository = resourceRepository;
            _emailService = emailService;
            _reassignmentService = reassignmentService;
>>>>>>> sebas
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

<<<<<<< HEAD
    // Nuevo método para reasignar recursos (basado en el diagrama)
    [HttpPost("reassign")]
    public async Task<ActionResult<bool>> ReassignResources([FromBody] ReassignResourcesRequest request)
    {
        var result = await _resourceService.ReassignResourcesAsync(request.ResourceIds, request.NewEventIds);
        return Ok(result);
    }
=======
        [HttpPost]
        public async Task<ActionResult<Resource>> Create(Resource resource)
        {
            resource.CreatedBy = resource.CreatedBy ?? "System";
            resource.UpdatedBy = resource.UpdatedBy ?? "System";

            var result = await _resourceRepository.AddAsync(resource);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
>>>>>>> sebas

    [HttpGet("available")]
    public async Task<ActionResult<List<ResourceDto>>> GetAvailableResources()
    {
        var resources = await _resourceService.GetAvailableResourcesAsync();
        return Ok(resources);
    }

<<<<<<< HEAD
    public class ReassignResourcesRequest
    {
        public List<Guid> ResourceIds { get; set; } = new();
        public List<Guid> NewEventIds { get; set; } = new();
=======
            resource.UpdatedBy = resource.UpdatedBy ?? "System";
            await _resourceRepository.UpdateAsync(resource);
            return NoContent();
        }

        [HttpPut("{id}/availability")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromBody] bool availability)
        {
            var resource = await _resourceRepository.GetByIdAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            resource.Availability = availability;
            resource.UpdatedBy = "System";
            await _resourceRepository.UpdateAsync(resource);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _resourceRepository.DeleteAsync(id);
            return NoContent();
        }

        // Revisar estado de disponibilidad
        [HttpGet("disponibilidad")]
        public async Task<IActionResult> CheckAvailability(string tipoEquipo, int cantidad, DateTime fecha)
        {
            var allResources = await _resourceRepository.GetAllAsync();
            var availableResources = allResources
                .Where(r => r.Type == tipoEquipo &&
                            r.FechaInicio <= fecha &&
                            r.FechaFin >= fecha)
                .ToList();
            bool isAvailable = availableResources.Any(r => r.Capacity >= cantidad);
            return Ok(new { Disponible = isAvailable });
        }

        [HttpPost("reservar")]
        public async Task<IActionResult> ReserveResource(int resourceId, int cantidad, string organizadorEmail)
        {
            var resource = await _resourceRepository.GetByIdAsync(resourceId);
            if (resource == null || resource.Capacity < cantidad)
                return BadRequest("Stock insuficiente");

            resource.Capacity -= cantidad;
            await _resourceRepository.UpdateAsync(resource);

            await _emailService.SendNotificationAsync(organizadorEmail, "Recurso reservado");
            return Ok();
        }
        [HttpGet("{id}/suggestions")]
        public async Task<IActionResult> GetSuggestions(int id, DateTime desiredTime)
        {
            var resource = await _resourceRepository.GetByIdAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            var resourceSuggestions = await _reassignmentService.GetResourceSuggestions(id, desiredTime);
            var timeSuggestions = await _reassignmentService.GetTimeSuggestions(id, desiredTime);

            return Ok(new
            {
                OriginalResource = resource,
                ResourceSuggestions = resourceSuggestions,
                TimeSuggestions = timeSuggestions
            });
        }

        [HttpGet("availability-with-suggestions")]
        public async Task<IActionResult> CheckAvailabilityWithSuggestions(string tipoEquipo, int cantidad, DateTime fecha)
        {
            // Verificar disponibilidad como antes
            var allResources = await _resourceRepository.GetAllAsync();
            var availableResources = allResources
                .Where(r => r.Type == tipoEquipo &&  
                            r.FechaInicio <= fecha &&
                            r.FechaFin >= fecha &&
                            r.Capacity >= cantidad)
                .ToList();

            if (availableResources.Any())
            {
                return Ok(new 
                {
                    Available = true,
                    Resources = availableResources
                });
            }

            // Si no hay disponibles, buscar sugerencias
            var potentialResources = allResources
                .Where(r => r.Type == tipoEquipo)
                .ToList();

            var suggestions = new List<object>();

            foreach (var resource in potentialResources)
            {
                var timeSuggestions = await _reassignmentService.GetTimeSuggestions(resource.Id, fecha);
                if (timeSuggestions.Any())
                {
                    suggestions.Add(new
                    {
                        Resource = resource,
                        AvailableTimes = timeSuggestions
                    });
                }
            }

            return Ok(new
            {
                Available = false,
                Suggestions = suggestions
            });
        }
>>>>>>> sebas
    }
}