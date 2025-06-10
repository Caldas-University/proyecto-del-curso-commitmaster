using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace EventLogistics.Application.Services
{
    public class ReassignmentService : IReassignmentService
    {
        private readonly IReassignmentRuleRepository _ruleRepository;
        private readonly IRepository<ResourceAssignment> _assignmentRepository;
        private readonly IRepository<Resource> _resourceRepository;
        private readonly NotificationService _notificationService;
        private readonly IConfiguration _configuration;

        public ReassignmentService(
            IReassignmentRuleRepository ruleRepository,
            IRepository<ResourceAssignment> assignmentRepository,
            IRepository<Resource> resourceRepository,
            NotificationService notificationService,
            IConfiguration configuration)
        {
            _ruleRepository = ruleRepository;
            _assignmentRepository = assignmentRepository;
            _resourceRepository = resourceRepository;
            _notificationService = notificationService;
            _configuration = configuration;
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
                    StartTime = currentAssignment.StartTime,
                    EndTime = currentAssignment.EndTime,
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
        
        private string BuildAdvancedNotificationContent(ResourceAssignment oldAssignment, ResourceAssignment newAssignment)
        {
            return $"La asignación de recurso ha cambiado de {oldAssignment.Resource?.Type} " +
                   $"a {newAssignment.Resource?.Name} para el evento {oldAssignment.Event?.Place}";
        }
        
        private string BuildNewAssignmentContent(ResourceAssignment assignment)
        {
            return $"Se te ha asignado el recurso {assignment.Resource?.Type} " +
                   $"para el evento {assignment.Event?.Place}";
        }

        private async Task SendReassignmentNotifications(ResourceAssignment oldAssignment, ResourceAssignment newAssignment)
        {
            // Notificación para el organizador
            var organizerNotification = new Notification
            {
                RecipientId = oldAssignment.AssignedToUserId.Value,
                Content = BuildAdvancedNotificationContent(oldAssignment, newAssignment),
                Status = "Pending",
                Channel = "Email", // o configurar según preferencias del usuario
                NotificationType = "ResourceReassigned"
            };

            await _notificationService.SendAdvancedNotification(
                organizerNotification,
                oldAssignment,
                "ResourceReassigned");

            // Notificación para el nuevo asignado (si es diferente)
            if (newAssignment.AssignedToUserId != oldAssignment.AssignedToUserId)
            {
                var newUserNotification = new Notification
                {
                    RecipientId = newAssignment.AssignedToUserId.Value,
                    Content = BuildNewAssignmentContent(newAssignment),
                    Status = "Pending",
                    Channel = "Email",
                    NotificationType = "NewAssignment"
                };

                await _notificationService.SendAdvancedNotification(
                    newUserNotification,
                    newAssignment,
                    "NewAssignment");
            }
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
        public Dictionary<string, object> EvaluateImpact(int eventId, int resourceId)
        {
            var impact = new Dictionary<string, object>();

            // Calcular tiempo de resolución, recursos afectados, etc.
            impact["affectedUsers"] = 3; // Ejemplo
            impact["alternativesAvailable"] = 2; // Ejemplo
            impact["estimatedTimeToResolve"] = "10 minutos"; // Ejemplo

            return impact;
        }

        public async Task<List<ResourceSuggestion>> GetResourceSuggestions(int resourceId, DateTime desiredTime)
        {
            var originalResource = await _resourceRepository.GetByIdAsync(resourceId);
            if (originalResource == null)
                return new List<ResourceSuggestion>();

            // Usar GetValue con valores por defecto para evitar errores
            var maxTimeDiff = _configuration.GetValue<int>("SuggestionSettings:MaxTimeDifferenceMinutes", 60);
            var maxSuggestions = _configuration.GetValue<int>("SuggestionSettings:MaxAlternativeSuggestions", 5);
            var similarityThreshold = _configuration.GetValue<double>("SuggestionSettings:SimilarResourceThreshold", 0.5);

            // Obtener todos los recursos del mismo tipo o similares
            var allResources = await _resourceRepository.GetAllAsync();

            // Filtrar recursos disponibles y compatibles
            var suggestions = allResources
                .Where(r => r.Id != resourceId &&
                           r.Availability &&
                           (r.Type == originalResource.Type ||
                            CalculateResourceSimilarity(r, originalResource) >= similarityThreshold))
                .Select(r => new ResourceSuggestion
                {
                    Resource = r,
                    SimilarityScore = CalculateResourceSimilarity(r, originalResource),
                    AvailableSlots = FindAvailableSlots(r, desiredTime, maxTimeDiff)
                })
                .Where(s => s.AvailableSlots.Any())
                .OrderByDescending(s => s.SimilarityScore)
                .ThenBy(s => s.AvailableSlots.Min(slot => Math.Abs((slot - desiredTime).TotalMinutes)))
                .Take(maxSuggestions)
                .ToList();

            return suggestions;
        }

        public async Task<List<TimeSuggestion>> GetTimeSuggestions(int resourceId, DateTime desiredTime)
        {
            var resource = await _resourceRepository.GetByIdAsync(resourceId);
            if (resource == null)
                return new List<TimeSuggestion>();

            // Usar GetValue con valores por defecto
            var maxTimeDiff = _configuration.GetValue<int>("SuggestionSettings:MaxTimeDifferenceMinutes", 60);
            var lookaheadDays = _configuration.GetValue<int>("SuggestionSettings:DefaultLookaheadDays", 1);

            var availableSlots = FindAvailableSlots(resource, desiredTime, maxTimeDiff, lookaheadDays);

            return availableSlots
                .Select(slot => new TimeSuggestion
                {
                    SuggestedTime = slot,
                    TimeDifference = (slot - desiredTime).TotalMinutes
                })
                .OrderBy(s => Math.Abs(s.TimeDifference))
                .ToList();
        }

        private List<DateTime> FindAvailableSlots(Resource resource, DateTime desiredTime, int maxTimeDifferenceMinutes, int lookaheadDays = 1)
        {
            // Lógica para encontrar slots disponibles cerca del tiempo deseado
            // Esto debería consultar las asignaciones existentes para el recurso
            // y encontrar huecos disponibles

            // Implementación simplificada para ejemplo:
            var availableSlots = new List<DateTime>();

            // Agregar el tiempo deseado si está disponible
            if (IsTimeAvailable(resource, desiredTime))
            {
                availableSlots.Add(desiredTime);
            }

            // Buscar slots alternativos dentro del rango permitido
            for (int minutes = 15; minutes <= maxTimeDifferenceMinutes; minutes += 15)
            {
                var earlierTime = desiredTime.AddMinutes(-minutes);
                if (IsTimeAvailable(resource, earlierTime))
                {
                    availableSlots.Add(earlierTime);
                }

                var laterTime = desiredTime.AddMinutes(minutes);
                if (IsTimeAvailable(resource, laterTime))
                {
                    availableSlots.Add(laterTime);
                }

                if (availableSlots.Count >= 3) break;
            }

            return availableSlots;
        }

        private bool IsTimeAvailable(Resource resource, DateTime time)
        {
            // Lógica para verificar si el recurso está disponible en el tiempo especificado
            // Deberías consultar las asignaciones existentes para este recurso

            // Implementación simplificada para ejemplo:
            return time >= resource.FechaInicio && time <= resource.FechaFin;
        }

        private double CalculateResourceSimilarity(Resource a, Resource b)
        {
            // Lógica para calcular qué tan similares son dos recursos
            // Basado en tipo, características, tags, etc.

            double similarity = 0;

            if (a.Type == b.Type) similarity += 0.5;

            // Comparar otras características...

            return similarity;
        }

        // Implementación corregida del método de la interfaz
        Task<Dictionary<string, object>> IReassignmentService.EvaluateImpact(int eventId, int resourceId)
        {
            var result = EvaluateImpact(eventId, resourceId);
            return Task.FromResult(result);
        }

        public async Task<ReassignmentResult> ModifyAssignment(int assignmentId, int newQuantity, DateTime? newStartTime)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
            if (assignment == null)
                return ReassignmentResult.FailedResult("Asignación no encontrada"); // Cambiado a FailedResult
        
            // Validar stock si cambia la cantidad
            if (newQuantity != 0)
            {
                var resource = await _resourceRepository.GetByIdAsync(assignment.ResourceId);
                if (resource.Capacity < newQuantity)
                    return ReassignmentResult.FailedResult("Stock insuficiente"); // Cambiado a FailedResult
            }
        
            // Actualizar propiedades
            assignment.IsModified = true;
            if (newQuantity > 0) assignment.Resource.Capacity -= newQuantity;
            if (newStartTime.HasValue) assignment.StartTime = newStartTime.Value;
        
            await _assignmentRepository.UpdateAsync(assignment);
        
            // Notificar
            await _notificationService.SendNotification(new Notification
            {
                RecipientId = assignment.AssignedToUserId.Value,
                Content = $"Su reserva fue modificada. Nueva cantidad: {newQuantity}",
                Status = "Delivered"
            });
        
            return ReassignmentResult.SuccessResult(assignment); // Cambiado a SuccessResult
        }

        public async Task<ReassignmentResult> ReassignAutomatically(int assignmentId, string reason)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
            if (assignment == null)
                return ReassignmentResult.FailedResult("Asignación no encontrada");

            // Lógica de reasignación automática (similar a ProcessResourceChange)
            var suggestions = await GetResourceSuggestions(assignment.ResourceId, assignment.StartTime);
            if (suggestions.Count == 0)
                return ReassignmentResult.FailedResult("No hay recursos alternativos");

            var newResource = suggestions.First().Resource;
            var newAssignment = new ResourceAssignment
            {
                // Copiar propiedades de la asignación original
                EventId = assignment.EventId,
                ResourceId = newResource.Id,
                StartTime = assignment.StartTime,
                EndTime = assignment.EndTime,
                Status = "Reasignado",
                OriginalAssignmentId = assignment.Id,
                ModificationReason = reason
            };

            await _assignmentRepository.AddAsync(newAssignment);
            assignment.Status = "Cancelado";
            await _assignmentRepository.UpdateAsync(assignment);

            return ReassignmentResult.SuccessResult(newAssignment);
        }

        public class ResourceSuggestion
        {
            public Resource Resource { get; set; }
            public double SimilarityScore { get; set; }
            public List<DateTime> AvailableSlots { get; set; } = new List<DateTime>();
        }

        public class TimeSuggestion
        {
            public DateTime SuggestedTime { get; set; }
            public double TimeDifference { get; set; } // en minutos
        }
    }
}