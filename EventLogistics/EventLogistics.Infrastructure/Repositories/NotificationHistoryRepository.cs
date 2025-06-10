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
    // Notification History Repository Implementation
    public class NotificationHistoryRepository : Repository<NotificationHistory>, INotificationHistoryRepository
    {
        public NotificationHistoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<NotificationHistory>> GetByNotificationIdAsync(int notificationId)
        {
            return await _dbSet
                .Where(nh => nh.NotificationId == notificationId)
                .OrderByDescending(nh => nh.ActionTimestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<NotificationHistory>> GetByActionAsync(string action)
        {
            return await _dbSet
                .Where(nh => nh.Action == action)
                .OrderByDescending(nh => nh.ActionTimestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<NotificationHistory>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _dbSet
                .Where(nh => nh.ActionTimestamp >= start && nh.ActionTimestamp <= end)
                .OrderByDescending(nh => nh.ActionTimestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<NotificationHistory>> GetByAssignmentIdAsync(int assignmentId)
        {
            return await _dbSet
                .Where(nh => nh.RelatedAssignmentId == assignmentId)
                .OrderByDescending(nh => nh.ActionTimestamp)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<NotificationHistory>> GetByNotificationTypeAsync(string type)
        {
            return await _dbSet
                .Where(nh => nh.NotificationType == type)
                .OrderByDescending(nh => nh.ActionTimestamp)
                .ToListAsync();
        }
    }
}