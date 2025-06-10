using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventLogistics.Infrastructure.Repositories
{
    public class AssignmentRepository : Repository<ResourceAssignment>, IAssignmentRepository
    {
        public AssignmentRepository(EventLogisticsDbContext context) : base(context)
        {
        }        public async Task<IEnumerable<ResourceAssignment>> GetByResourceIdAsync(Guid resourceId)
        {
            return await _dbSet
                .Where(a => a.ResourceId == resourceId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ResourceAssignment>> GetByEventIdAsync(Guid eventId)
        {
            return await _dbSet
                .Where(a => a.EventId == eventId)
                .ToListAsync();
        }
    }
}