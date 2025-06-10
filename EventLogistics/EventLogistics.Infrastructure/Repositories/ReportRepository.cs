using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventLogistics.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly EventLogisticsDbContext _context;

        public ReportRepository(EventLogisticsDbContext context)
        {
            _context = context;
        }        public async Task<IEnumerable<Resource>> GenerateReportAsync(Guid? eventId, string? resourceType, string? status)
        {
            var query = _context.Resources.AsQueryable();

            if (!string.IsNullOrEmpty(resourceType))
            {
                query = query.Where(r => r.Type == resourceType);
            }

            if (!string.IsNullOrEmpty(status))
            {
                // Asumiendo que status se relaciona con disponibilidad
                bool isAvailable = status.ToLower() == "disponible";
                query = query.Where(r => r.Availability == isAvailable);
            }            if (eventId.HasValue)
            {
                // Filtrar por recursos asignados a un evento específico usando ResourceAssignments
                query = query.Where(r => _context.ResourceAssignments
                    .Any(a => a.ResourceId == r.Id && a.EventId == eventId.Value));
            }

            return await query.ToListAsync();
        }
    }
}