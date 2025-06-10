using EventLogistics.Domain.Entities;

namespace EventLogistics.Application.Interfaces;
public interface IEventService
{
    Task<Event?> GetByIdAsync(Guid eventId);
    // Puedes agregar más métodos según tus necesidades
    Task<List<Event>> GetAllAsync();
    Task<Event> CreateAsync(Event newEvent);
    Task<Event> UpdateAsync(Event updatedEvent);
    Task<bool> DeleteAsync(Guid eventId);
    Task<List<Event>> GetEventsByStatusAsync(string status);
    Task<List<Event>> GetEventsByPlaceAsync(string place);
}