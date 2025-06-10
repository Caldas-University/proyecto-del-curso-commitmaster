using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventLogistics.Domain.Entities;

namespace EventLogistics.Domain.Repositories
{
    public interface IIncidentSolutionRepository
    {
        Task AddAsync(IncidentSolution solution);
        Task<IncidentSolution> GetByIdAsync(Guid id);
        Task<IEnumerable<IncidentSolution>> GetByIncidentIdAsync(Guid incidentId);
        Task UpdateAsync(IncidentSolution solution);
        Task DeleteAsync(Guid id);
    }
}
