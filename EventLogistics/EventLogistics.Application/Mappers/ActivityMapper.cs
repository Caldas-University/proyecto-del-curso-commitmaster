using EventLogistics.Domain.Entities;
using EventLogistics.Application.DTOs;

namespace EventLogistics.Application.Mappers
{
    public static class ActivityMapper
    {
        public static ActivityResponseDto ToResponseDto(Activity activity)
        {
            return new ActivityResponseDto
            {
                Id = activity.Id,
                Name = activity.Name,
                Description = activity.Description,
                StartTime = activity.StartTime,
                EndTime = activity.EndTime
                // Agrega más campos si tu DTO los necesita
            };
        }
    }
}
