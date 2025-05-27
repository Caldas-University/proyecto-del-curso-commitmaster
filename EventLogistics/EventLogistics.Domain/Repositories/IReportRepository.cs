using EventLogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Domain.Repositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<ResourceAssignment>> GetFilteredAssignments(int? eventId, string? resourceType, string? status);
    }
}