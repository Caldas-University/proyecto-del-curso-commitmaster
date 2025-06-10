using EventLogistics.Domain.Entities;

public interface IConflictValidationService
{
    Task<ConflictValidationResult> ValidateActivityConflicts(int eventId, DateTime startTime, DateTime endTime, int? excludeActivityId = null);
    Task<ConflictValidationResult> ValidateResourceConflicts(int resourceId, DateTime startTime, DateTime endTime);
}

public class ConflictValidationResult
{
    public bool HasConflicts { get; set; }
    public IEnumerable<Activity> ConflictingActivities { get; set; }
    public IEnumerable<ResourceAssignment> ConflictingAssignments { get; set; }
}