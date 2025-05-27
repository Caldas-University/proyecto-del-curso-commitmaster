using EventLogistics.Domain.DTOs;
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
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ResourceAssignment>> GetFilteredAssignments(int? eventId, string? resourceType, string? status)
        {
            var query = _context.Set<ResourceAssignment>().AsQueryable();

            if (eventId.HasValue)
                query = query.Where(a => a.EventId == eventId.Value);

            if (!string.IsNullOrEmpty(resourceType))
                query = query.Where(a => a.Resource.Type == resourceType);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(a => a.Status == status);

            return await query
                .Include(a => a.Resource)
                .Include(a => a.Event)
                .ToListAsync();
        }

        public async Task<IEnumerable<ResourceMetricsDto>> GetResourceMetricsAsync()
        {
            var resources = await _context.Resources
                .Include(r => r.Assignments)
                .ThenInclude(a => a.Event)
                .ToListAsync();

            var metrics = resources.Select(r => new ResourceMetricsDto
            {
                Name = r.Type, // No hay propiedad Name, así que usamos Type como identificador
                Type = r.Type,
                TotalCount = r.Capacity, // Usamos Capacity como cantidad total
                UsedCount = r.Assignments.Count(a => a.Status == "Asignado"),
                AvailableCount = r.Availability ? r.Capacity : 0,
                EventsCount = r.Assignments.Select(a => a.EventId).Distinct().Count(),
                ActivitiesCount = r.Assignments.Count,
                TotalUsage = r.Assignments.Count,
                Availability = r.Availability
            }).ToList();

            return metrics;
        }
    }
}