using Microsoft.AspNetCore.Mvc;
using EventLogistics.Application.Interfaces;
using EventLogistics.Application.DTOs;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceServiceApp _resourceService;
        private readonly IResourceRepository _resourceRepository;

        public ResourceController(IResourceServiceApp resourceService, IResourceRepository resourceRepository)
        {
            _resourceService = resourceService;
            _resourceRepository = resourceRepository;
        }

        /// <summary>
        /// Obtiene todos los recursos disponibles
        /// </summary>
        /// <returns>Lista de recursos disponibles</returns>
        [HttpGet("available")]
        public async Task<ActionResult<List<ResourceDto>>> GetAvailableResources()
        {
            try
            {
                var resources = await _resourceService.GetAvailableResourcesAsync();
                return Ok(resources);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene todos los recursos
        /// </summary>
        /// <returns>Lista de todos los recursos</returns>
        [HttpGet]
        public async Task<ActionResult<List<ResourceDto>>> GetAllResources()
        {
            try
            {
                var resources = await _resourceRepository.GetAllAsync();
                var resourceDtos = resources.Select(r => new ResourceDto
                {
                    Id = r.Id,
                    Type = r.Type,
                    Availability = r.Availability,
                    Capacity = r.Capacity,
                    Assignments = r.Assignments ?? new List<Guid>()
                }).ToList();

                return Ok(resourceDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un recurso por ID
        /// </summary>
        /// <param name="id">ID del recurso</param>
        /// <returns>Datos del recurso</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceDto>> GetResourceById(Guid id)
        {
            try
            {
                var resource = await _resourceService.CheckAvailabilityAsync(id);
                if (resource == null)
                {
                    return NotFound(new { message = "Recurso no encontrado" });
                }

                return Ok(resource);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo recurso
        /// </summary>
        /// <param name="createResourceDto">Datos del recurso a crear</param>
        /// <returns>Recurso creado</returns>
        [HttpPost]
        public async Task<ActionResult<ResourceDto>> CreateResource([FromBody] CreateResourceDto createResourceDto)
        {
            try
            {
                if (createResourceDto == null)
                {
                    return BadRequest(new { message = "Los datos del recurso son requeridos" });
                }

                if (string.IsNullOrWhiteSpace(createResourceDto.Name))
                {
                    return BadRequest(new { message = "El nombre del recurso es requerido" });
                }

                if (string.IsNullOrWhiteSpace(createResourceDto.Type))
                {
                    return BadRequest(new { message = "El tipo del recurso es requerido" });
                }

                if (createResourceDto.Capacity <= 0)
                {
                    return BadRequest(new { message = "La capacidad debe ser mayor que 0" });
                }

                var resource = new Resource
                {
                    Name = createResourceDto.Name,
                    Type = createResourceDto.Type,
                    Capacity = createResourceDto.Capacity,
                    Availability = createResourceDto.Availability,
                    FechaInicio = createResourceDto.FechaInicio ?? DateTime.UtcNow,
                    FechaFin = createResourceDto.FechaFin ?? DateTime.UtcNow.AddDays(365),
                    Tags = createResourceDto.Tags ?? new List<string>(),
                    Assignments = new List<Guid>()
                };

                var createdResource = await _resourceRepository.AddAsync(resource);

                var resourceDto = new ResourceDto
                {
                    Id = createdResource.Id,
                    Type = createdResource.Type,
                    Availability = createdResource.Availability,
                    Capacity = createdResource.Capacity,
                    Assignments = createdResource.Assignments ?? new List<Guid>()
                };

                return CreatedAtAction(nameof(GetResourceById), new { id = createdResource.Id }, resourceDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza el estado de un recurso
        /// </summary>
        /// <param name="id">ID del recurso</param>
        /// <param name="updateStatusDto">Nuevo estado del recurso</param>
        /// <returns>Recurso actualizado</returns>
        [HttpPut("{id}/status")]
        public async Task<ActionResult<ResourceDto>> UpdateResourceStatus(Guid id, [FromBody] UpdateResourceStatusDto updateStatusDto)
        {
            try
            {
                if (updateStatusDto == null || string.IsNullOrWhiteSpace(updateStatusDto.Status))
                {
                    return BadRequest(new { message = "El estado es requerido" });
                }

                var resource = await _resourceService.UpdateResourceStatusAsync(id, updateStatusDto.Status);
                if (resource == null)
                {
                    return NotFound(new { message = "Recurso no encontrado" });
                }

                return Ok(resource);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Asigna un recurso a un evento
        /// </summary>
        /// <param name="id">ID del recurso</param>
        /// <param name="assignResourceDto">Datos de la asignación</param>
        /// <returns>Resultado de la asignación</returns>
        [HttpPost("{id}/assign")]
        public async Task<ActionResult> AssignResource(Guid id, [FromBody] AssignResourceDto assignResourceDto)
        {
            try
            {
                if (assignResourceDto == null || assignResourceDto.EventId == Guid.Empty)
                {
                    return BadRequest(new { message = "El ID del evento es requerido" });
                }

                var result = await _resourceService.AssignResourceAsync(id, assignResourceDto.EventId);
                if (!result)
                {
                    return BadRequest(new { message = "No se pudo asignar el recurso al evento" });
                }

                return Ok(new { message = "Recurso asignado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un recurso
        /// </summary>
        /// <param name="id">ID del recurso</param>
        /// <returns>Resultado de la eliminación</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteResource(Guid id)
        {
            try
            {
                var resource = await _resourceRepository.GetByIdAsync(id);
                if (resource == null)
                {
                    return NotFound(new { message = "Recurso no encontrado" });
                }

                await _resourceRepository.DeleteAsync(id);
                return Ok(new { message = "Recurso eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Verifica la disponibilidad de un recurso
        /// </summary>
        /// <param name="id">ID del recurso</param>
        /// <returns>Estado de disponibilidad del recurso</returns>
        [HttpGet("{id}/availability")]
        public async Task<ActionResult<ResourceDto>> CheckResourceAvailability(Guid id)
        {
            try
            {
                var resource = await _resourceService.CheckAvailabilityAsync(id);
                if (resource == null)
                {
                    return NotFound(new { message = "Recurso no encontrado" });
                }

                return Ok(resource);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }

    // DTOs adicionales para el controlador
    public class CreateResourceDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public bool Availability { get; set; } = true;
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public List<string>? Tags { get; set; }
    }

    public class UpdateResourceStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }

    public class AssignResourceDto
    {
        public Guid EventId { get; set; }
    }
}