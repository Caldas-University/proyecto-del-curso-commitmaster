using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventLogistics.Infrastructure.Repositories
{
    // Notification Repository Implementation
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetByRecipientAsync(int userId)
        {
            return await _dbSet
                .Where(n => n.RecipientId == userId)
                .OrderByDescending(n => n.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Where(n => n.Status == status)
                .OrderByDescending(n => n.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnconfirmedNotificationsAsync()
        {
            return await _dbSet
                .Where(n => !n.Confirmation)
                .OrderByDescending(n => n.Timestamp)
                .ToListAsync();
        }
    }
}