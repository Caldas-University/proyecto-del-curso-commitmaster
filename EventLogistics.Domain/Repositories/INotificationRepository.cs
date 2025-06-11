using EventLogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Domain.Repositories
{
    // Notification Repository Interface
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByRecipientAsync(Guid userId);
        Task<IEnumerable<Notification>> GetByStatusAsync(string status);
        Task<IEnumerable<Notification>> GetUnconfirmedNotificationsAsync();

        Task<IEnumerable<Notification>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        Task<IEnumerable<Notification>> GetByRecipientIdAsync(Guid recipientId);
    }
}