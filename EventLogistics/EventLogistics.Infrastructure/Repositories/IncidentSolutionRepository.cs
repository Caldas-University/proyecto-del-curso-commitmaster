using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventLogistics.Infrastructure.Repositories
{
    public class IncidentSolutionRepository : IIncidentSolutionRepository
    {
        private readonly EventLogisticsDbContext _context;

        public IncidentSolutionRepository(EventLogisticsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(IncidentSolution solution)
        {
            _context.IncidentSolutions.Add(solution);
            await _context.SaveChangesAsync();
        }

        public async Task<IncidentSolution> GetByIdAsync(Guid id)
        {
            return await _context.IncidentSolutions.FindAsync(id);
        }

        public async Task<IEnumerable<IncidentSolution>> GetByIncidentIdAsync(Guid incidentId)
        {
            return await _context.IncidentSolutions
                .Where(s => s.IncidentId == incidentId)
                .ToListAsync();
        }

        public async Task UpdateAsync(IncidentSolution solution)
        {
            _context.IncidentSolutions.Update(solution);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var solution = await _context.IncidentSolutions.FindAsync(id);
            if (solution != null)
            {
                _context.IncidentSolutions.Remove(solution);
                await _context.SaveChangesAsync();
            }
        }

        // Implementa aquí los otros métodos requeridos por la interfaz si los hay
    }
}
