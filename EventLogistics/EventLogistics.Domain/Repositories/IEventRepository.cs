namespace EventLogistics.Domain.Repositories;

using EventLogistics.Domain.Entities;

public interface IEventRepository
{
    Task<Event?> GetEventAsync(Guid id);
    Task<List<Event>> GetAllAsync();
    Task<Event> GetByIdAsync(Guid id);
}
