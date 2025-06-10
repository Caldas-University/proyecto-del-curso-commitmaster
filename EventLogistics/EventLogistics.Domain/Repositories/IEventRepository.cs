<<<<<<< HEAD
using System.Collections.Generic;
using System.Threading.Tasks;
using EventLogistics.Domain.Entities;

namespace EventLogistics.Domain.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventsByLocationAsync(int locationId);
        Task<Event> GetEventWithDetailsAsync(int id);
    }
}
=======
namespace EventLogistics.Domain.Repositories;

using EventLogistics.Domain.Entities;

public interface IEventRepository
{
    Task<Event?> GetEventAsync(Guid id);
    Task<List<Event>> GetAllAsync();
}
>>>>>>> d2804b8fb33a385d35d1dddc2534720d8e145fe9
