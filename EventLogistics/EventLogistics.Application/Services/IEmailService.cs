namespace EventLogistics.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendNotificationAsync(string toEmail, string message);
    }
}