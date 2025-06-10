using EventLogistics.Domain.Entities;
using EventLogistics.Application.DTOs;
using static EventLogistics.Application.Services.ReassignmentService;

namespace EventLogistics.Application.Interfaces
{
    public interface IReassignmentService
    {
        Task<bool> ProcessResourceChange(int resourceId, bool newAvailability);
        Task<Dictionary<string, object>> EvaluateImpact(int eventId, int resourceId);
        Task<List<ResourceSuggestionDto>> GetResourceSuggestions(int resourceId, DateTime desiredTime);
        Task<List<TimeSuggestionDto>> GetTimeSuggestions(int resourceId, DateTime desiredTime);
        
        // Nuevos métodos
        Task<ReassignmentResult> ModifyAssignment(int assignmentId, int newQuantity, DateTime? newStartTime);
        Task<ReassignmentResult> ReassignAutomatically(int assignmentId, string reason);
    }

    public class ReassignmentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ResourceAssignment UpdatedAssignment { get; set; }

        public static ReassignmentResult SuccessResult(ResourceAssignment assignment) 
            => new() { Success = true, UpdatedAssignment = assignment };
        
        public static ReassignmentResult FailedResult(string message) 
            => new() { Success = false, Message = message };
    }
}