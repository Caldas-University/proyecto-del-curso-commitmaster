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