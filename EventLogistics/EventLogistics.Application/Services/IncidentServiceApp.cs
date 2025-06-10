using EventLogistics.Application.Contracts.Services;
using EventLogistics.Application.DTOs;
using EventLogistics.Application.Mappers;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;

namespace EventLogistics.Application.Services
{
    public class IncidentServiceApp : IIncidentServiceApp
    {
        private readonly IIncidentRepository _incidentRepository;
        private readonly IIncidentSolutionRepository _incidentSolutionRepository;

        public IncidentServiceApp(IIncidentRepository incidentRepository,
          IIncidentSolutionRepository incidentSolutionRepository)
        {
            _incidentRepository = incidentRepository;
            _incidentSolutionRepository = incidentSolutionRepository;
        }

        public async Task<Guid> CreateIncidentAsync(Guid eventId, string description, string location, DateTime incidentDate)
        {
            var incident = new Incident
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                Description = description,
                Location = location,
                IncidentDate = incidentDate,
                Status = "Open"
            };

            await _incidentRepository.AddAsync(incident);
            return incident.Id;
        }

        public async Task UpdateIncidentAsync(Guid incidentId, string description, string location, DateTime incidentDate)
        {
            var incident = await _incidentRepository.GetByIdAsync(incidentId);
            if (incident == null) return;

            incident.Description = description;
            incident.Location = location;
            incident.IncidentDate = incidentDate;

            await _incidentRepository.UpdateAsync(incident);
        }

        public async Task DeleteIncidentAsync(Guid incidentId)
        {
            await _incidentRepository.DeleteAsync(incidentId);
        }

        public async Task<IncidentDto?> GetIncidentByIdAsync(Guid incidentId)
        {
            var incident = await _incidentRepository.GetByIdAsync(incidentId);
            return incident != null ? IncidentMapper.ToDto(incident) : null;
        }

        public async Task<IEnumerable<IncidentDto>> GetIncidentsByEventIdAsync(Guid eventId)
        {
            var incidents = await _incidentRepository.GetByEventIdAsync(eventId);
            return incidents.Select(IncidentMapper.ToDto);
        }

        public async Task<Guid> ApplyIncidentSolutionAsync(Guid incidentId, string actionTaken, string appliedBy)
        {
            var solution = new IncidentSolution
            {
                Id = Guid.NewGuid(),
                IncidentId = incidentId,
                ActionTaken = actionTaken,
                AppliedBy = appliedBy,
                DateApplied = DateTime.UtcNow
            };

            await _incidentSolutionRepository.AddAsync(solution);

            var incident = await _incidentRepository.GetByIdAsync(incidentId);
            if (incident != null)
            {
                incident.Status = "Resolved";
                await _incidentRepository.UpdateAsync(incident);
            }

            return solution.Id;
        }

        public async Task<IncidentSolution?> GetIncidentSolutionByIdAsync(Guid id)
        {
            return await _incidentSolutionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<IncidentSolution>> GetSolutionsByIncidentIdAsync(Guid incidentId)
        {
            return await _incidentSolutionRepository.GetByIncidentIdAsync(incidentId);
        }

        public async Task UpdateIncidentSolutionAsync(Guid id, IncidentSolution solution)
        {
            if (id != solution.Id)
                throw new ArgumentException("ID mismatch");
                
            await _incidentSolutionRepository.UpdateAsync(solution);
        }

        public async Task DeleteIncidentSolutionAsync(Guid id)
        {
            await _incidentSolutionRepository.DeleteAsync(id);
        }
    }
}