namespace EventLogistics.Application.Interfaces;

using EventLogistics.Application.DTOs;
using System.Threading.Tasks;

public interface INotificationServiceApp
{
    Task<NotificationDto> GenerateNotificationAsync(Guid recipientId, string content);
    Task<bool> SendCommunicationAsync(Guid notificationId);
    Task<bool> ConfirmReceptionAsync(Guid notificationId);
    Task RegisterLogAsync(string message);
    Task<Dictionary<string, double>> CalculateMetricsAsync();
}
