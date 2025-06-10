using EventLogistics.Application.Contracts.Services;
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
                IncidentDate = incidentDate
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

        public async Task<Incident?> GetIncidentByIdAsync(Guid incidentId)
        {
            return await _incidentRepository.GetByIdAsync(incidentId);
        }

        public async Task<IEnumerable<Incident>> GetIncidentsByEventIdAsync(Guid eventId)
        {
            return await _incidentRepository.GetByEventIdAsync(eventId);
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
    }
}