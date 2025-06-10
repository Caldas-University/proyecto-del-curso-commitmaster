using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventLogistics.Application.DTOs;
using EventLogistics.Domain.Entities;

namespace EventLogistics.Application.Contracts.Services
{
    public interface IIncidentServiceApp
    {
        Task<Guid> CreateIncidentAsync(
            Guid eventId,
            string description,
            string location,
            DateTime incidentDate);

        Task<IncidentDto?> GetIncidentByIdAsync(Guid id);
        Task<IEnumerable<IncidentDto>> GetIncidentsByEventIdAsync(Guid eventId);
        Task UpdateIncidentAsync(
            Guid id,
            string description,
            string location,
            DateTime incidentDate);
        Task DeleteIncidentAsync(Guid id);
        
        // Métodos para soluciones
        Task<Guid> ApplyIncidentSolutionAsync(Guid incidentId, string actionTaken, string appliedBy);
        Task<IncidentSolution?> GetIncidentSolutionByIdAsync(Guid id);
        Task<IEnumerable<IncidentSolution>> GetSolutionsByIncidentIdAsync(Guid incidentId);
        Task UpdateIncidentSolutionAsync(Guid id, IncidentSolution solution);
        Task DeleteIncidentSolutionAsync(Guid id);
    }
}