using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Application.Services
{
    public class NotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationHistoryRepository _historyRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly IConfiguration _configuration;

        public NotificationService(
            INotificationRepository notificationRepository,
            INotificationHistoryRepository historyRepository,
            IAssignmentRepository assignmentRepository,
            IEmailService emailService,
            ISmsService smsService,
            IConfiguration configuration)
        {
            _notificationRepository = notificationRepository;
            _historyRepository = historyRepository;
            _emailService = emailService;
            _smsService = smsService;
            _configuration = configuration;
        }

        public async Task<Notification> SendNotification(Notification notification)
        {
            // 1. Guardar la notificación
            var savedNotification = await _notificationRepository.AddAsync(notification);

            // 2. Registrar en el historial
            var history = new NotificationHistory
            {
                NotificationId = savedNotification.Id,
                Action = "Generated",
                ActionTimestamp = DateTime.UtcNow,
                Details = "Notification generated by the system",
                Result = "Success",
                CreatedBy = "System",
                UpdatedBy = "System"
            };

            await _historyRepository.AddAsync(history);

            // 3. Envío real (simulado aquí)
            await SendActualNotification(savedNotification);

            return savedNotification;
        }

        private async Task SendActualNotification(Notification notification)
        {
            // Simular envío de la notificación (en un sistema real, aquí integrarías con SMS, email, etc.)
            await Task.Delay(100); // Simular envío

            // Registrar el envío en el historial
            var history = new NotificationHistory
            {
                NotificationId = notification.Id,
                Action = "Sent",
                ActionTimestamp = DateTime.UtcNow,
                Details = "Notification sent via preferred channel",
                Result = "Success",
                CreatedBy = "System",
                UpdatedBy = "System"
            };

            await _historyRepository.AddAsync(history);

            // Actualizar estado de la notificación
            notification.Status = "Sent";
            await _notificationRepository.UpdateAsync(notification);
        }        public async Task<bool> ConfirmNotification(Guid notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
                return false;

            // Actualizar estado
            notification.Status = "Confirmed";
            await _notificationRepository.UpdateAsync(notification);

            // Registrar en el historial
            var history = new NotificationHistory
            {
                NotificationId = notification.Id,
                Action = "Confirmed",
                ActionTimestamp = DateTime.UtcNow,
                Details = "Notification confirmed by recipient",
                Result = "Success",
                CreatedBy = "System",
                UpdatedBy = "System"
            };

            await _historyRepository.AddAsync(history);

            return true;
        }

        public async Task<Dictionary<string, object>> CalculateMetrics(DateTime startDate, DateTime endDate)
        {
            var metrics = new Dictionary<string, object>();

            var allNotifications = await _notificationRepository.GetByDateRangeAsync(startDate, endDate);
            var allHistories = await _historyRepository.GetByDateRangeAsync(startDate, endDate);

            // Calcular métricas
            int totalSent = 0;
            int totalConfirmed = 0;
            TimeSpan averageResponseTime = TimeSpan.Zero;

            foreach (var notification in allNotifications)
            {
                var sentRecord = allHistories.FirstOrDefault(h => h.NotificationId == notification.Id && h.Action == "Sent");
                var confirmedRecord = allHistories.FirstOrDefault(h => h.NotificationId == notification.Id && h.Action == "Confirmed");

                if (sentRecord != null)
                {
                    totalSent++;

                    if (confirmedRecord != null)
                    {
                        totalConfirmed++;
                        averageResponseTime += confirmedRecord.ActionTimestamp - sentRecord.ActionTimestamp;
                    }
                }
            }

            if (totalConfirmed > 0)
            {
                averageResponseTime = TimeSpan.FromTicks(averageResponseTime.Ticks / totalConfirmed);
            }

            // Guardar métricas
            metrics["totalSent"] = totalSent;
            metrics["totalConfirmed"] = totalConfirmed;
            metrics["confirmationRate"] = totalSent > 0 ? (double)totalConfirmed / totalSent * 100 : 0;
            metrics["averageResponseTime"] = averageResponseTime.ToString(@"hh\:mm\:ss");

            return metrics;
        }
        private string GetTemplateForNotification(string notificationType)
        {
            // Implementar lógica para obtener plantilla basada en el tipo
            return _configuration[$"NotificationSettings:Templates:{notificationType}:Email"];
        }

        private List<string> GetChannelsForNotificationType(string notificationType)
        {
            // Ejemplo básico - puedes hacerlo más sofisticado
            return new List<string> { "Email" };
        }

        private async Task RecordDeliveryFailure(Notification notification, ResourceAssignment assignment, string channel, string error)
        {
            var history = new NotificationHistory
            {
                NotificationId = notification.Id,
                RelatedAssignmentId = assignment.Id,
                Action = "Failed",
                ActionTimestamp = DateTime.UtcNow,
                Details = $"Failed to deliver via {channel}. Error: {error}",
                Channel = channel,
                Result = "Failed"
            };
            await _historyRepository.AddAsync(history);
        }
        public async Task SendAdvancedNotification(Notification notification, ResourceAssignment assignment, string notificationType)
        {
            // 1. Guardar la notificación base
            var savedNotification = await _notificationRepository.AddAsync(notification);

            // 2. Registrar en el historial con detalles avanzados
            var history = new NotificationHistory
            {
                NotificationId = savedNotification.Id,
                RelatedAssignmentId = assignment.Id,
                NotificationType = notificationType,
                Action = "Generated",
                ActionTimestamp = DateTime.UtcNow,
                Details = $"Advanced notification of type {notificationType} generated",
                Channel = notification.Channel, // Nuevo campo en Notification
                TemplateName = GetTemplateForNotification(notificationType),
                Result = "Pending"
            };

            await _historyRepository.AddAsync(history);

            // 3. Enviar por los canales configurados
            await SendThroughConfiguredChannels(savedNotification, assignment, notificationType);
        }

        private async Task SendThroughConfiguredChannels(Notification notification, ResourceAssignment assignment, string notificationType)
        {
            var channels = GetChannelsForNotificationType(notificationType);

            foreach (var channel in channels)
            {
                try
                {
                    switch (channel)
                    {
                        case "Email":
                            await _emailService.SendAssignmentChangeEmail(assignment, notificationType);
                            break;
                        case "SMS":
                            await _smsService.SendAssignmentChangeSms(assignment, notificationType);
                            break;
                            // Otros canales...
                    }

                    await RecordDeliverySuccess(notification, assignment, channel);
                }
                catch (Exception ex)
                {
                    await RecordDeliveryFailure(notification, assignment, channel, ex.Message);
                }
            }
        }

        private async Task RecordDeliverySuccess(Notification notification, ResourceAssignment assignment, string channel)
        {
            var history = new NotificationHistory
            {
                NotificationId = notification.Id,
                RelatedAssignmentId = assignment.Id,
                Action = "Delivered",
                ActionTimestamp = DateTime.UtcNow,
                Details = $"Successfully delivered via {channel}",
                Channel = channel,
                Result = "Success"
            };

            await _historyRepository.AddAsync(history);

            notification.Status = "Delivered";
            await _notificationRepository.UpdateAsync(notification);
        }        public async Task<IEnumerable<NotificationHistory>> GetNotificationHistoryForAssignment(Guid assignmentId)
        {
            return await _historyRepository.GetByAssignmentIdAsync(assignmentId);
        }public async Task<bool> ResendNotification(Guid notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null) return false;

            var history = await _historyRepository.GetByNotificationIdAsync(notificationId);
            var assignmentId = history.FirstOrDefault()?.RelatedAssignmentId;

            if (!assignmentId.HasValue) return false;

            var assignment = await _assignmentRepository.GetByIdAsync(assignmentId.Value);
            if (assignment == null) return false;

            await SendAdvancedNotification(notification, assignment, notification.NotificationType);
            return true;
        }
    }
}