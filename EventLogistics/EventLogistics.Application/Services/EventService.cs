using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;

namespace EventLogistics.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }        public async Task<List<EventDto>> GetAllAsync()        {
            var events = await _eventRepository.GetAllAsync();
            return events.Select(ev => new EventDto
            {
                Id = ev.Id,
                Name = ev.Name,
                Place = ev.Place,
                Schedule = ev.Schedule,
                Resources = ev.Resources.Select(r => r.Id).ToList(),
                Status = ev.Status
            }).ToList();
        }public async Task<EventDto?> GetEventByIdAsync(Guid id)
        {
            var ev = await _eventRepository.GetByIdAsync(id);
            if (ev == null) return null;            return new EventDto
            {
                Id = ev.Id,
                Name = ev.Name,
                Place = ev.Place,
                Schedule = ev.Schedule,
                Resources = ev.Resources.Select(r => r.Id).ToList(),
                Status = ev.Status
            };
        }        public async Task<EventDto> CreateAsync(EventDto newEventDto)
        {
            var newEvent = new Event 
            {
                Name = newEventDto.Name,
                Place = newEventDto.Place,
                Schedule = newEventDto.Schedule,
                Status = newEventDto.Status,
                Resources = new List<ResourceAssignment>(),
                Activities = new List<Activity>()
            };
            var createdEvent = await _eventRepository.AddAsync(newEvent);
              return new EventDto
            {
                Id = createdEvent.Id,
                Name = createdEvent.Name,
                Place = createdEvent.Place,
                Schedule = createdEvent.Schedule,
                Resources = createdEvent.Resources.Select(r => r.Id).ToList(),
                Status = createdEvent.Status
            };
        }        public async Task<EventDto> UpdateAsync(EventDto updatedEventDto)
        {
            var existingEvent = await _eventRepository.GetByIdAsync(updatedEventDto.Id);
            if (existingEvent == null)
                throw new InvalidOperationException("Event not found");

            // Actualizar propiedades (necesitarías añadir métodos de actualización en la entidad)
            await _eventRepository.UpdateAsync(existingEvent);
              return new EventDto
            {
                Id = existingEvent.Id,
                Name = existingEvent.Name,
                Place = existingEvent.Place,
                Schedule = existingEvent.Schedule,
                Resources = existingEvent.Resources.Select(r => r.Id).ToList(),
                Status = existingEvent.Status
            };
        }

        public async Task<bool> DeleteAsync(Guid eventId)
        {
            try
            {
                await _eventRepository.DeleteAsync(eventId);
                return true;
            }
            catch
            {
                return false;
            }
        }        public async Task<List<EventDto>> GetEventsByStatusAsync(string status)
        {
            var events = await _eventRepository.GetAllAsync();
            var filteredEvents = events.Where(e => e.Status == status);
            
            return filteredEvents.Select(ev => new EventDto
            {
                Id = ev.Id,
                Name = ev.Name,
                Place = ev.Place,
                Schedule = ev.Schedule,
                Resources = ev.Resources.Select(r => r.Id).ToList(),
                Status = ev.Status
            }).ToList();        }public async Task<List<EventDto>> GetEventsByPlaceAsync(string place)
        {
            var events = await _eventRepository.GetAllAsync();
            var filteredEvents = events.Where(e => e.Place == place);
            
            return filteredEvents.Select(ev => new EventDto
            {
                Id = ev.Id,
                Name = ev.Name,
                Place = ev.Place,
                Schedule = ev.Schedule,
                Resources = ev.Resources.Select(r => r.Id).ToList(),
                Status = ev.Status
            }).ToList();
        }
    }
}