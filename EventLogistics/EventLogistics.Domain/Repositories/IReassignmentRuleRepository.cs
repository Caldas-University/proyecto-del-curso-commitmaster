using EventLogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Domain.Repositories
{
    // ReassignmentRule Repository Interface
    public interface IReassignmentRuleRepository : IRepository<ReassignmentRule>
    {
        Task<IEnumerable<ReassignmentRule>> GetActiveRulesAsync();
        Task<IEnumerable<ReassignmentRule>> GetRulesByResourceTypeAsync(Guid resourceTypeId);
        Task<IEnumerable<ReassignmentRule>> GetRulesByPriorityAsync(int minimumPriority);
    }
}