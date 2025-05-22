using EventLogistics.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Domain.Repositories
{
    public interface IActivityRepository : IRepository<Activity>
    {
        Task<IEnumerable<Activity>> GetByEventIdAsync(int eventId);
        Task<Activity> GetByNameAsync(string name);
        Task<IEnumerable<Activity>> GetByParticipantIdAsync(int participantId);
    }
}
