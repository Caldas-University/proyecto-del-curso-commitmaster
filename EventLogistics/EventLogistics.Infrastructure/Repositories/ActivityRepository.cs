using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventLogistics.Infrastructure.Repositories
{
    // Activity Repository Implementation
    public class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        public ActivityRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Activity>> GetByEventIdAsync(int eventId)
        {
            return await _dbSet
                .Where(a => a.EventId == eventId)
                .ToListAsync();
        }

        public async Task<Activity?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.Name == name);
        }

        public async Task<IEnumerable<Activity>> GetByParticipantIdAsync(int participantId)
        {
            return await _dbSet
                .Where(a => a.ParticipantActivities.Any(pa => pa.ParticipantId == participantId))
                .ToListAsync();
        }
    }
}
