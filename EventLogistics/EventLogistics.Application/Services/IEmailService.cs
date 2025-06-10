using EventLogistics.Domain.Entities;

namespace EventLogistics.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendNotificationAsync(string toEmail, string message);
        Task SendAssignmentChangeEmail(ResourceAssignment assignment, string notificationType); 
        Task SendTemplateEmailAsync(string toEmail, string templateName, object templateData); 
    }
}