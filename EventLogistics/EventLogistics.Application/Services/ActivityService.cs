using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventLogistics.Domain.Repositories;
using EventLogistics.Application.DTOs;
using EventLogistics.Application.Mappers;

public interface IActivityService
{
    Task<IEnumerable<ActivityResponseDto>> GetAllAsync();
    Task<ActivityResponseDto?> GetByIdAsync(int id);
}

public class ActivityService : IActivityService
{
    private readonly IActivityRepository _activityRepository;

    public ActivityService(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<IEnumerable<ActivityResponseDto>> GetAllAsync()
    {
        var activities = await _activityRepository.GetAllAsync();
        return activities.Select(ActivityMapper.ToResponseDto);
    }

    public async Task<ActivityResponseDto?> GetByIdAsync(int id)
    {
        var activity = await _activityRepository.GetByIdAsync(id);
        return activity == null ? null : ActivityMapper.ToResponseDto(activity);
    }
}