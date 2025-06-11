using EventLogistics.Application.DTOs;

namespace EventLogistics.Application.Interfaces;

public interface IEventService
{
    Task<EventDto?> GetEventByIdAsync(Guid eventId);
    Task<List<EventDto>> GetAllAsync();
    Task<EventDto> CreateAsync(CreateEventRequest newEvent);
    Task<EventDto> UpdateAsync(EventDto updatedEvent);
    Task<bool> DeleteAsync(Guid eventId);
    Task<List<EventDto>> GetEventsByStatusAsync(string status);
    Task<List<EventDto>> GetEventsByPlaceAsync(string place);
}