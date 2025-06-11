namespace EventLogistics.Domain.Repositories;

using EventLogistics.Domain.Entities;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid id);
    Task<List<Event>> GetAllAsync();
    Task<Event> AddAsync(Event eventEntity);
    Task UpdateAsync(Event eventEntity);
    Task DeleteAsync(Guid id);
}
