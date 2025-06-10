using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        Task<Incident?> GetIncidentByIdAsync(Guid id);
        Task<IEnumerable<Incident>> GetIncidentsByEventIdAsync(Guid eventId);
        Task UpdateIncidentAsync(
            Guid id,
            string description,
            string location,
            DateTime incidentDate);
        Task DeleteIncidentAsync(Guid id);
    }
}