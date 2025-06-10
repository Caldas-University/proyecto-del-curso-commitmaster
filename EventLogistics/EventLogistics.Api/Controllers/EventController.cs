using EventLogistics.Application.Services;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IConflictValidationService _conflictService;
        private readonly IResourceAssignmentRepository _assignmentRepository;

        public EventController(
            IRepository<Event> eventRepository,
            IActivityRepository activityRepository,
            IConflictValidationService conflictService,
            IResourceAssignmentRepository assignmentRepository)
        {
            _eventRepository = eventRepository;
            _activityRepository = activityRepository;
            _conflictService = conflictService;
            _assignmentRepository = assignmentRepository;
        }

        // Endpoint para crear una nueva actividad en un evento
        [HttpPost("{eventId}/activities")]
        public async Task<IActionResult> CreateActivity(int eventId, [FromBody] ActivityCreateRequest request)
        {
            // Validar que el evento exista
            var eventEntity = await _eventRepository.GetByIdAsync(eventId);
            if (eventEntity == null)
            {
                return NotFound($"Evento con ID {eventId} no encontrado");
            }

            // Validar conflictos de horario con otras actividades del mismo evento
            var scheduleValidation = await _conflictService.ValidateActivityConflicts(
                eventId, request.StartTime, request.EndTime);

            if (scheduleValidation.HasConflicts)
            {
                return Conflict(new
                {
                    Message = "Existen conflictos de horario con otras actividades",
                    ConflictingActivities = scheduleValidation.ConflictingActivities
                });
            }

            // Validar conflictos de recursos solicitados
            var resourceConflicts = new List<ResourceConflictResult>();
            foreach (var resourceAssignment in request.ResourceAssignments)
            {
                var resourceValidation = await _conflictService.ValidateResourceConflicts(
                    resourceAssignment.ResourceId, request.StartTime, request.EndTime);

                if (resourceValidation.HasConflicts)
                {
                    resourceConflicts.Add(new ResourceConflictResult
                    {
                        ResourceId = resourceAssignment.ResourceId,
                        ConflictingAssignments = resourceValidation.ConflictingAssignments
                    });
                }
            }

            if (resourceConflicts.Count > 0)
            {
                return Conflict(new
                {
                    Message = "Existen conflictos con recursos solicitados",
                    ResourceConflicts = resourceConflicts
                });
            }

            // Crear la nueva actividad
            var activity = new Activity
            {
                Name = request.Name,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                EventId = eventId,
                OrganizatorId = request.OrganizatorId,
                ResourceAssignments = new List<ResourceAssignment>()
            };

            // Asignar recursos
            foreach (var assignmentRequest in request.ResourceAssignments)
            {
                activity.ResourceAssignments.Add(new ResourceAssignment
                {
                    ResourceId = assignmentRequest.ResourceId,
                    Quantity = assignmentRequest.Quantity,
                    AssignmentDate = DateTime.UtcNow,
                    EventId = eventId,
                    Activity = activity
                });
            }

            await _activityRepository.AddAsync(activity);

            return CreatedAtAction(nameof(GetActivity), 
                new { eventId = eventId, activityId = activity.Id }, 
                activity);
        }

        // Endpoint para actualizar una actividad existente
        [HttpPut("{eventId}/activities/{activityId}")]
        public async Task<IActionResult> UpdateActivity(
            int eventId, int activityId, [FromBody] ActivityUpdateRequest request)
        {
            var activity = await _activityRepository.GetByIdAsync(activityId);
            if (activity == null || activity.EventId != eventId)
            {
                return NotFound();
            }

            // Validar conflictos excluyendo la actividad actual
            var scheduleValidation = await _conflictService.ValidateActivityConflicts(
                eventId, request.StartTime, request.EndTime, activityId);

            if (scheduleValidation.HasConflicts)
            {
                return Conflict(new
                {
                    Message = "Existen conflictos de horario con otras actividades",
                    ConflictingActivities = scheduleValidation.ConflictingActivities
                });
            }

            // Validar conflictos de recursos
            var resourceConflicts = new List<ResourceConflictResult>();
            foreach (var resourceAssignment in request.ResourceAssignments)
            {
                var resourceValidation = await _conflictService.ValidateResourceConflicts(
                    resourceAssignment.ResourceId, request.StartTime, request.EndTime);

                if (resourceValidation.HasConflicts)
                {
                    // Filtrar conflictos que no sean de la actividad actual
                    var externalConflicts = resourceValidation.ConflictingAssignments
                        .Where(a => a.ActivityId != activityId);

                    if (externalConflicts.Any())
                    {
                        resourceConflicts.Add(new ResourceConflictResult
                        {
                            ResourceId = resourceAssignment.ResourceId,
                            ConflictingAssignments = externalConflicts
                        });
                    }
                }
            }

            if (resourceConflicts.Count > 0)
            {
                return Conflict(new
                {
                    Message = "Existen conflictos con recursos solicitados",
                    ResourceConflicts = resourceConflicts
                });
            }

            // Actualizar la actividad
            activity.Name = request.Name;
            activity.StartTime = request.StartTime;
            activity.EndTime = request.EndTime;
            activity.OrganizatorId = request.OrganizatorId;

            // Actualizar asignaciones de recursos (simplificado)
            await UpdateResourceAssignments(activity, request.ResourceAssignments);

            await _activityRepository.UpdateAsync(activity);

            return NoContent();
        }

        private async Task UpdateResourceAssignments(Activity activity, List<ResourceAssignmentRequest> requests)
        {
            // Eliminar asignaciones que ya no están en la solicitud
            var assignmentsToRemove = activity.ResourceAssignments
                .Where(ra => !requests.Any(r => r.ResourceId == ra.ResourceId))
                .ToList();

            foreach (var assignment in assignmentsToRemove)
            {
                await _assignmentRepository.DeleteAsync(assignment.Id);
            }

            // Actualizar o agregar nuevas asignaciones
            foreach (var request in requests)
            {
                var existingAssignment = activity.ResourceAssignments
                    .FirstOrDefault(ra => ra.ResourceId == request.ResourceId);

                if (existingAssignment != null)
                {
                    existingAssignment.Quantity = request.Quantity;
                    await _assignmentRepository.UpdateAsync(existingAssignment);
                }
                else
                {
                    var newAssignment = new ResourceAssignment
                    {
                        ResourceId = request.ResourceId,
                        Quantity = request.Quantity,
                        AssignmentDate = DateTime.UtcNow,
                        EventId = activity.EventId,
                        ActivityId = activity.Id
                    };
                    await _assignmentRepository.AddAsync(newAssignment);
                }
            }
        }

        // Endpoint para obtener una actividad específica
        [HttpGet("{eventId}/activities/{activityId}")]
        public async Task<ActionResult<Activity>> GetActivity(int eventId, int activityId)
        {
            var activity = await _activityRepository.GetByIdAsync(activityId);
            if (activity == null || activity.EventId != eventId)
            {
                return NotFound();
            }

            return activity;
        }

        // Endpoint para validar conflictos sin crear la actividad
        [HttpPost("{eventId}/activities/validate")]
        public async Task<IActionResult> ValidateActivity(int eventId, [FromBody] ActivityValidationRequest request)
        {
            var validationResult = new ActivityValidationResult();

            // Validar conflictos de horario
            var scheduleValidation = await _conflictService.ValidateActivityConflicts(
                eventId, request.StartTime, request.EndTime, request.ExcludeActivityId);

            validationResult.HasScheduleConflicts = scheduleValidation.HasConflicts;
            validationResult.ConflictingActivities = scheduleValidation.ConflictingActivities;

            // Validar conflictos de recursos
            validationResult.ResourceConflicts = new List<ResourceConflictResult>();
            foreach (var resourceId in request.ResourceIds)
            {
                var resourceValidation = await _conflictService.ValidateResourceConflicts(
                    resourceId, request.StartTime, request.EndTime);

                if (resourceValidation.HasConflicts)
                {
                    // Filtrar conflictos que no sean de la actividad actual (si se está editando)
                    var externalConflicts = request.ExcludeActivityId.HasValue
                        ? resourceValidation.ConflictingAssignments
                            .Where(a => a.ActivityId != request.ExcludeActivityId.Value)
                        : resourceValidation.ConflictingAssignments;

                    if (externalConflicts.Any())
                    {
                        validationResult.ResourceConflicts.Add(new ResourceConflictResult
                        {
                            ResourceId = resourceId,
                            ConflictingAssignments = externalConflicts
                        });
                    }
                }
            }

            return Ok(validationResult);
        }
    }

    // Clases DTO para las solicitudes
    public class ActivityCreateRequest
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int OrganizatorId { get; set; }
        public List<ResourceAssignmentRequest> ResourceAssignments { get; set; }
    }

    public class ActivityUpdateRequest
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int OrganizatorId { get; set; }
        public List<ResourceAssignmentRequest> ResourceAssignments { get; set; }
    }

    public class ActivityValidationRequest
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<int> ResourceIds { get; set; }
        public int? ExcludeActivityId { get; set; }
    }

    public class ResourceAssignmentRequest
    {
        public int ResourceId { get; set; }
        public int Quantity { get; set; }
    }

    public class ActivityValidationResult
    {
        public bool HasScheduleConflicts { get; set; }
        public IEnumerable<Activity> ConflictingActivities { get; set; }
        public List<ResourceConflictResult> ResourceConflicts { get; set; }
    }

    public class ResourceConflictResult
    {
        public int ResourceId { get; set; }
        public IEnumerable<ResourceAssignment> ConflictingAssignments { get; set; }
    }
}