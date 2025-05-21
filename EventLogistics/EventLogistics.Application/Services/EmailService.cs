using System.Net;
using System.Net.Mail;
using EventLogistics.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EventLogistics.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
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
    }
}