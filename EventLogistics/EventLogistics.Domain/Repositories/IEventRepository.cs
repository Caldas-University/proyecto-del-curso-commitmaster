namespace EventLogistics.Domain.Repositories;

using EventLogistics.Domain.Entities;

public interface IEventRepository
{
    Task<Event?> GetEventAsync(Guid id);
}
