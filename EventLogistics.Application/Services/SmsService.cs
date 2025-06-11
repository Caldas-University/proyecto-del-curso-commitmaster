using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace EventLogistics.Application.Services
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAssignmentChangeSms(ResourceAssignment assignment, string notificationType)
        {
            string templatePath = _configuration[$"NotificationSettings:Templates:{notificationType}:SMS"];
            string templateContent = await File.ReadAllTextAsync(templatePath);
            
            string message = templateContent
                .Replace("{ResourceName}", assignment.Resource?.Name ?? "recurso")
                .Replace("{EventName}", assignment.Event?.Name ?? "evento");
            
            await SendSmsAsync(assignment.AssignedTo.PhoneNumber, message); // Cambiar de AssignedToUser a AssignedTo
        }

        public async Task SendSmsAsync(string phoneNumber, string message)
        {
            // Implementación real con Twilio u otro proveedor SMS
            // Ejemplo básico:
            var accountSid = _configuration["NotificationSettings:SmsSettings:AccountSid"];
            var authToken = _configuration["NotificationSettings:SmsSettings:AuthToken"];
            
            // Código para enviar SMS (dependerá del proveedor)
        }
    }
}