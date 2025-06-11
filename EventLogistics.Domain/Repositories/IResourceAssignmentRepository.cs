using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;

public interface IResourceAssignmentRepository : IRepository<ResourceAssignment>
{
    Task<IEnumerable<ResourceAssignment>> GetByResourceIdAsync(Guid resourceId);
    Task<IEnumerable<ResourceAssignment>> GetConflictingAssignments(Guid resourceId, DateTime startTime, DateTime endTime);
    Task<IEnumerable<ResourceAssignment>> GetConflictingAssignmentsAsync(Guid resourceId, DateTime startTime, DateTime endTime);
}