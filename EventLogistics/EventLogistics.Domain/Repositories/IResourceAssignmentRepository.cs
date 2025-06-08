using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;

public interface IResourceAssignmentRepository : IRepository<ResourceAssignment>
{
    Task<IEnumerable<ResourceAssignment>> GetByResourceIdAsync(int resourceId);
    Task<IEnumerable<ResourceAssignment>> GetConflictingAssignments(int resourceId, DateTime startTime, DateTime endTime);
    Task<IEnumerable<ResourceAssignment>> GetConflictingAssignmentsAsync(int resourceId, DateTime startTime, DateTime endTime);
}