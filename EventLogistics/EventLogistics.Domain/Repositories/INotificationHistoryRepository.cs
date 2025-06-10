using EventLogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Domain.Repositories
{
    // NotificationHistory Repository Interface
    public interface INotificationHistoryRepository : IRepository<NotificationHistory>
    {        Task<IEnumerable<NotificationHistory>> GetByNotificationIdAsync(Guid notificationId);
        Task<IEnumerable<NotificationHistory>> GetByActionAsync(string action);
        Task<IEnumerable<NotificationHistory>> GetByDateRangeAsync(DateTime start, DateTime end);
        Task<IEnumerable<NotificationHistory>> GetByAssignmentIdAsync(Guid assignmentId);
        Task<IEnumerable<NotificationHistory>> GetByNotificationTypeAsync(string type);
    }
}