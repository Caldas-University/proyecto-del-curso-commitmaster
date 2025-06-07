using static EventLogistics.Application.Services.ReassignmentService;

namespace EventLogistics.Application.Interfaces
{
    public interface IReassignmentService
    {
        Task<bool> ProcessResourceChange(int resourceId, bool newAvailability);
        Task<Dictionary<string, object>> EvaluateImpact(int eventId, int resourceId);
        Task<List<ResourceSuggestion>> GetResourceSuggestions(int resourceId, DateTime desiredTime);
        Task<List<TimeSuggestion>> GetTimeSuggestions(int resourceId, DateTime desiredTime);
    }
}