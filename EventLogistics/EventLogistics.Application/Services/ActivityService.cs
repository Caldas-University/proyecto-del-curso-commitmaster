using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;

namespace EventLogistics.Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityService(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }        public async Task<ActivityDto> CreateAsync(CreateActivityRequest activityRequest)
        {
            var activity = new Activity(
                activityRequest.EventId,
                activityRequest.OrganizatorId,
                activityRequest.Name,
                activityRequest.Place,
                activityRequest.StartTime,
                activityRequest.EndTime,
                activityRequest.Status
            );

            await _activityRepository.AddAsync(activity);

            return new ActivityDto
            {
                Id = activity.Id,
                EventId = activity.EventId,
                OrganizatorId = activity.OrganizatorId,
                Name = activity.Name,
                Place = activity.Place,
                StartTime = activity.StartTime,
                EndTime = activity.EndTime,
                Status = activity.Status
            };
        }

        public async Task<ActivityDto?> GetByIdAsync(Guid id)
        {
            var activity = await _activityRepository.GetByIdAsync(id);
            if (activity == null) return null;

            return new ActivityDto
            {
                Id = activity.Id,
                EventId = activity.EventId,
                OrganizatorId = activity.OrganizatorId,
                Name = activity.Name,
                Place = activity.Place,
                StartTime = activity.StartTime,
                EndTime = activity.EndTime,
                Status = activity.Status
            };
        }

        public async Task<List<ActivityDto>> GetAllAsync()
        {
            var activities = await _activityRepository.GetAllAsync();
            return activities.Select(a => new ActivityDto
            {
                Id = a.Id,
                EventId = a.EventId,
                OrganizatorId = a.OrganizatorId,
                Name = a.Name,
                Place = a.Place,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Status = a.Status
            }).ToList();
        }
    }
}
