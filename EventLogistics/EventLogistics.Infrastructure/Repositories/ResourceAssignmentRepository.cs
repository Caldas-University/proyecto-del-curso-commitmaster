using EventLogistics.Domain.Entities;
using EventLogistics.Infrastructure.Persistence;
using EventLogistics.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class ResourceAssignmentRepository : Repository<ResourceAssignment>, IResourceAssignmentRepository
{
    public ResourceAssignmentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<ResourceAssignment>> GetByResourceId(int resourceId)
    {
        return await _context.ResourceAssignments
            .Include(ra => ra.Activity)
            .Where(ra => ra.ResourceId == resourceId)
            .ToListAsync();
    }

    public Task<IEnumerable<ResourceAssignment>> GetByResourceIdAsync(int resourceId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ResourceAssignment>> GetConflictingAssignments(int resourceId, DateTime startTime, DateTime endTime)
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

    public Task<IEnumerable<ResourceAssignment>> GetConflictingAssignmentsAsync(int resourceId, DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }
}