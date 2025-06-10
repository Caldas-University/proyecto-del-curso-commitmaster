using EventLogistics.Domain.Entities;

namespace EventLogistics.Application.Interfaces
{
    public interface ISmsService
    {
        Task SendAssignmentChangeSms(ResourceAssignment assignment, string notificationType);
        Task SendSmsAsync(string phoneNumber, string message);
    }
}