public class ConflictValidationService : IConflictValidationService
{
    private readonly IActivityRepository _activityRepository;
    private readonly IResourceAssignmentRepository _assignmentRepository;

    public ConflictValidationService(
        IActivityRepository activityRepository,
        IResourceAssignmentRepository assignmentRepository)
    {
        _activityRepository = activityRepository;
        _assignmentRepository = assignmentRepository;
    }    public async Task<ConflictValidationResult> ValidateActivityConflicts(Guid eventId, DateTime startTime, DateTime endTime, Guid? excludeActivityId = null)
    {
        var conflicts = await _activityRepository.GetConflictingActivitiesAsync(eventId, startTime, endTime, excludeActivityId);
        
        return new ConflictValidationResult
        {
            HasConflicts = conflicts.Any(),
            ConflictingActivities = conflicts
        };
    }

    public async Task<ConflictValidationResult> ValidateResourceConflicts(Guid resourceId, DateTime startTime, DateTime endTime)
    {
        var conflicts = await _assignmentRepository.GetConflictingAssignmentsAsync(resourceId, startTime, endTime);
        
        return new ConflictValidationResult
        {
            HasConflicts = conflicts.Any(),
            ConflictingAssignments = conflicts
        };
    }
}