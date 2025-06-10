using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventLogistics.Infrastructure.Repositories
{
    public class AssignmentRepository : Repository<ResourceAssignment>, IAssignmentRepository
    {
        public AssignmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ResourceAssignment>> GetByResourceIdAsync(int resourceId)
        {
            return await _dbSet
                .Where(a => a.ResourceId == resourceId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ResourceAssignment>> GetByEventIdAsync(int eventId)
        {
            return await _dbSet
                .Where(a => a.EventId == eventId)
                .ToListAsync();
        }
    }
}