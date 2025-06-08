using System.Net;
using System.Net.Mail;
using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EventLogistics.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly IConfiguration _configuration;
        public EmailService(IOptions<SmtpSettings> smtpSettings, IConfiguration configuration)
        {
            _smtpSettings = smtpSettings.Value;
            _configuration = configuration;
        }

        public async Task SendNotificationAsync(string toEmail, string message)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            using var client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"]))
            {
                Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["FromEmail"]),
                Subject = "Notificación de Event Logistics",
                Body = message,
                IsBodyHtml = false
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
        public async Task SendAssignmentChangeEmail(ResourceAssignment assignment, string notificationType)
        {
            string templatePath = _configuration[$"NotificationSettings:Templates:{notificationType}:Email"];
            var templateData = new
            {
                Assignment = assignment,
                Event = assignment.Event,
                Resource = assignment.Resource,
                User = assignment.AssignedTo // Cambiar de AssignedToUser a AssignedTo
            };

            await SendTemplateEmailAsync(assignment.AssignedTo.Email, templatePath, templateData); // Usar AssignedTo
        }

        public async Task SendTemplateEmailAsync(string toEmail, string templateName, object templateData)
        {
            // 1. Cargar plantilla
            string templateContent = await LoadTemplate(templateName);
            
            // 2. Renderizar plantilla con los datos
            string renderedContent = RenderTemplate(templateContent, templateData);
            
            // 3. Enviar email
            await SendEmailAsync(toEmail, "Notificación de cambio", renderedContent);
        }

        private async Task<string> LoadTemplate(string templatePath)
        {
            // Implementación para cargar plantilla desde archivo
            return await File.ReadAllTextAsync(templatePath);
        }

        private string RenderTemplate(string template, object data)
        {
            // Implementación simple (podrías usar RazorLight o similar para más complejidad)
            foreach (var prop in data.GetType().GetProperties())
            {
                template = template.Replace($"{{{{{prop.Name}}}}}", prop.GetValue(data)?.ToString());
            }
            return template;
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            // Implementación real usando SmtpClient o SendGrid
            // Usar _smtpSettings para configuración
        }
    }
}