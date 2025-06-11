using EventLogistics.Domain.Entities;

public interface IConflictValidationService
{
    Task<ConflictValidationResult> ValidateActivityConflicts(Guid eventId, DateTime startTime, DateTime endTime, Guid? excludeActivityId = null);
    Task<ConflictValidationResult> ValidateResourceConflicts(Guid resourceId, DateTime startTime, DateTime endTime);
}

public class ConflictValidationResult
{
    public bool HasConflicts { get; set; }
    public IEnumerable<Activity> ConflictingActivities { get; set; }
    public IEnumerable<ResourceAssignment> ConflictingAssignments { get; set; }
}