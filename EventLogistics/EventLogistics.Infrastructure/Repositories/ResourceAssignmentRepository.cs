using EventLogistics.Domain.Entities;
using EventLogistics.Infrastructure.Persistence;
using EventLogistics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class ResourceAssignmentRepository : Repository<ResourceAssignment>, IResourceAssignmentRepository
{
    public ResourceAssignmentRepository(EventLogisticsDbContext context) : base(context) { }    public async Task<IEnumerable<ResourceAssignment>> GetByResourceId(Guid resourceId)
    {
        return await _context.ResourceAssignments
            .Include(ra => ra.Activity)
            .Where(ra => ra.ResourceId == resourceId)
            .ToListAsync();
    }

    public Task<IEnumerable<ResourceAssignment>> GetByResourceIdAsync(Guid resourceId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ResourceAssignment>> GetConflictingAssignments(Guid resourceId, DateTime startTime, DateTime endTime)
    {
        return await _context.ResourceAssignments
            .Include(ra => ra.Activity)
            .Where(ra => ra.ResourceId == resourceId && 
                   ra.Activity != null &&
                   ((startTime >= ra.Activity.StartTime && startTime < ra.Activity.EndTime) ||
                    (endTime > ra.Activity.StartTime && endTime <= ra.Activity.EndTime) ||
                    (startTime <= ra.Activity.StartTime && endTime >= ra.Activity.EndTime)))
            .ToListAsync();
    }

    public Task<IEnumerable<ResourceAssignment>> GetConflictingAssignmentsAsync(Guid resourceId, DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }
}