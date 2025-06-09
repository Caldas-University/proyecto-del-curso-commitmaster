using EventLogistics.Domain.Entities;

namespace EventLogistics.Application.Interfaces;
public interface IEventService
{
    Task<Event?> GetByIdAsync(Guid eventId);
    // Puedes agregar más métodos según tus necesidades
}