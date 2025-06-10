using EventLogistics.Application.DTOs;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;

namespace EventLogistics.Application.Services
{
    public class EventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<List<EventDto>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetAllAsync();
            return events.Select(ev => new EventDto
            {
                Id = ev.Id,
                Place = ev.Place,
                Schedule = ev.Schedule,
                Resources = ev.Resources,
                Status = ev.Status
            }).ToList();
        }

        public async Task<EventDto?> GetEventByIdAsync(Guid id)
        {
            var ev = await _eventRepository.GetByIdAsync(id);
            if (ev == null) return null;

            return new EventDto
            {
                Id = ev.Id,
                Place = ev.Place,
                Schedule = ev.Schedule,
                Resources = ev.Resources,
                Status = ev.Status
            };
        }
    }
}