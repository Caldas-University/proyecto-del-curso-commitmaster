
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventLogistics.Domain.Entities;

namespace EventLogistics.Domain.Repositories

{
    public interface IIncidentRepository
    {
        Task<Incident> GetByIdAsync(Guid id);
        Task<IEnumerable<Incident>> GetByEventIdAsync(Guid eventId);
        Task AddAsync(Incident incident);
        Task UpdateAsync(Incident incident);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Incident>> GetAllAsync();
    }
}
