using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventLogistics.Infrastructure.Repositories
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Attendance>> GetByParticipantIdAsync(int participantId)
        {
            return await _dbSet
                .Where(a => a.ParticipantId == participantId)
                .ToListAsync();
        }

        public async Task<Attendance?> GetByEventAndParticipantAsync(int eventId, int participantId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.EventId == eventId && a.ParticipantId == participantId);
        }

        public async Task<IEnumerable<Attendance>> GetByDateAsync(DateTime date)
        {
            return await _dbSet
                .Where(a => a.Timestamp.Date == date.Date)
                .ToListAsync();
        }
    }
}
