using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventLogistics.Application.Services
{
    public class ReassignmentService
    {
        private readonly IReassignmentRuleRepository _ruleRepository;
        private readonly IRepository<ResourceAssignment> _assignmentRepository;
        private readonly IRepository<Resource> _resourceRepository;
        private readonly NotificationService _notificationService;

        public ReassignmentService(
            IReassignmentRuleRepository ruleRepository,
            IRepository<ResourceAssignment> assignmentRepository,
            IRepository<Resource> resourceRepository,
            NotificationService notificationService)
        {
            _ruleRepository = ruleRepository;
            _assignmentRepository = assignmentRepository;
            _resourceRepository = resourceRepository;
            _notificationService = notificationService;
        }

        public async Task<bool> ProcessResourceChange(int resourceId, bool newAvailability)
        {
            // 1. Obtener el recurso
            var resource = await _resourceRepository.GetByIdAsync(resourceId);
            if (resource == null)
                return false;

            // 2. Si el recurso está pasando a no disponible, buscar asignaciones afectadas
            if (!newAvailability && resource.Availability)
            {
                // 2.1 Actualizar estado del recurso
                resource.Availability = newAvailability;
                await _resourceRepository.UpdateAsync(resource);

                // 2.2 Buscar asignaciones afectadas
                var assignments = await GetAssignmentsForResource(resourceId);
                
                // 2.3 Para cada asignación afectada, encontrar un recurso alternativo
                foreach (var assignment in assignments)
                {
                    await ReassignResource(assignment);
                }
                
                return true;
            }
            
            // 3. Si el recurso está pasando a disponible, solo actualizar su estado
            resource.Availability = newAvailability;
            await _resourceRepository.UpdateAsync(resource);
            
            return true;
        }

        private async Task<List<ResourceAssignment>> GetAssignmentsForResource(int resourceId)
        {
            // Implementación real: consultar base de datos para obtener todas las asignaciones para este recurso
            // Esta es una implementación simplificada para demo
            var allAssignments = await _assignmentRepository.GetAllAsync();
            return allAssignments.Where(a => a.ResourceId == resourceId).ToList();
        }

        private async Task ReassignResource(ResourceAssignment currentAssignment)
        {
            // Asegurarnos de cargar Resource si no está cargado
            if (currentAssignment.Resource == null)
            {
                // Cargar el recurso asociado
                var resource = await _resourceRepository.GetByIdAsync(currentAssignment.ResourceId);
                currentAssignment.Resource = resource;
            }

            // 1. Buscar reglas aplicables
            var rules = await _ruleRepository.GetActiveRulesAsync();
            rules = rules.Where(r => r.ResourceTypeId == null || 
                                    (currentAssignment.Resource != null && 
                                     r.ResourceTypeId == currentAssignment.Resource.Id))
                     .OrderBy(r => r.Priority)
                     .ToList();

            // 2. Buscar recursos alternativos disponibles
            string resourceType = currentAssignment.Resource?.Type ?? "Unknown";
            var alternativeResources = await FindAlternativeResources(resourceType);
            
            // 3. Si hay recursos alternativos, seleccionar uno según las reglas
            if (alternativeResources.Any())
            {
                var newResource = SelectBestResource(alternativeResources, rules);
                
                // 4. Crear nueva asignación
                var newAssignment = new ResourceAssignment
                {
                    EventId = currentAssignment.EventId,
                    ResourceId = newResource.Id,
                    StartTime = currentAssignment.StartTime, // Asegúrate de que estas propiedades existan
                    EndTime = currentAssignment.EndTime,     // en la clase ResourceAssignment
                    Status = "Reasignado",
                    AssignedToUserId = currentAssignment.AssignedToUserId,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                };
                
                await _assignmentRepository.AddAsync(newAssignment);
                
                // 5. Actualizar asignación anterior
                currentAssignment.Status = "Cancelado";
                await _assignmentRepository.UpdateAsync(currentAssignment);
                
                // 6. Enviar notificaciones
                await SendReassignmentNotifications(currentAssignment, newAssignment);
            }
            else
            {
                // No hay recursos alternativos, marcar como pendiente de resolución manual
                currentAssignment.Status = "Pendiente de reasignación";
                await _assignmentRepository.UpdateAsync(currentAssignment);
                
                // Notificar a organizadores sobre el problema
                await SendFailureNotifications(currentAssignment);
            }
        }

        private async Task<List<Resource>> FindAlternativeResources(string resourceType)
        {
            // Buscar recursos disponibles del mismo tipo
            var allResources = await _resourceRepository.GetAllAsync();
            return allResources.Where(r => r.Type == resourceType && r.Availability).ToList();
        }

        private Resource SelectBestResource(List<Resource> resources, IEnumerable<ReassignmentRule> rules)
        {
            // Implementación sencilla: seleccionar el primer recurso disponible
            // En una implementación completa, aplicaríamos las reglas de forma más sofisticada
            return resources.FirstOrDefault();
        }

        private async Task SendReassignmentNotifications(ResourceAssignment oldAssignment, ResourceAssignment newAssignment)
        {
            // 1. Notificar al organizador
            if (oldAssignment.AssignedToUserId.HasValue)
            {
                var notification = new Notification
                {
                    RecipientId = oldAssignment.AssignedToUserId.Value,
                    Content = $"El recurso {oldAssignment.Resource.Type} ha sido reemplazado por {newAssignment.Resource.Type} para el evento programado.",
                    Status = "Pending",
                    Timestamp = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                };
                
                await _notificationService.SendNotification(notification);
            }
            
            // 2. Notificar a otros involucrados (esto dependería de tu modelo de datos completo)
        }

        private async Task SendFailureNotifications(ResourceAssignment assignment)
        {
            // Notificar que no se pudo reasignar automáticamente
            if (assignment.AssignedToUserId.HasValue)
            {
                var notification = new Notification
                {
                    RecipientId = assignment.AssignedToUserId.Value,
                    Content = $"No se pudo reasignar automáticamente el recurso {assignment.Resource.Type} para el evento programado. Se requiere intervención manual.",
                    Status = "Urgent",
                    Timestamp = DateTime.UtcNow,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                };
                
                await _notificationService.SendNotification(notification);
            }
        }

        // Método para evaluar el impacto de una reasignación
        public async Task<Dictionary<string, object>> EvaluateImpact(int eventId, int resourceId)
        {
            var impact = new Dictionary<string, object>();
            
            // Calcular tiempo de resolución, recursos afectados, etc.
            impact["affectedUsers"] = 3; // Ejemplo
            impact["alternativesAvailable"] = 2; // Ejemplo
            impact["estimatedTimeToResolve"] = "10 minutos"; // Ejemplo
            
            return impact;
        }
    }
}