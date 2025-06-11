using EventLogistics.Application.DTOs;

namespace EventLogistics.Application.Interfaces
{
    public interface IActivityService
    {
        Task<ActivityDto> CreateAsync(CreateActivityRequest activityRequest);
        Task<ActivityDto?> GetByIdAsync(Guid id);
        Task<List<ActivityDto>> GetAllAsync();
    }
}
