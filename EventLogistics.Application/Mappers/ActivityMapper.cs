using EventLogistics.Application.DTOs;
using EventLogistics.Domain.Entities;
using System.Collections.Generic;

namespace EventLogistics.Application.Mappers
{
    public static class ActivityMapper
    {
        /// <summary>
        /// Convierte una entidad Activity a su DTO.
        /// </summary>
        public static ActivityDto ToDto(Activity activity)
        {
            if (activity == null) return null!;
            return new ActivityDto
            {
                Id = activity.Id,
                Name = activity.Name,
                Place = activity.Place,
                StartTime = activity.StartTime,
                EndTime = activity.EndTime,
                Status = activity.Status,
                EventId = activity.EventId,
                // OrganizatorId no está en la entidad Activity, así que puedes omitirlo o poner Guid.Empty
                OrganizatorId = Guid.Empty
            };
        }

        /// <summary>
        /// Convierte una lista de entidades Activity a una lista de DTOs.
        /// </summary>
        public static List<ActivityDto> ToDtoList(IEnumerable<Activity> activities)
        {
            var result = new List<ActivityDto>();
            foreach (var activity in activities)
            {
                result.Add(ToDto(activity));
            }
            return result;
        }
    }
}