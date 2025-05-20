using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventLogistics.Infrastructure.Repositories
{
    // Reassignment Rule Repository Implementation
    public class ReassignmentRuleRepository : Repository<ReassignmentRule>, IReassignmentRuleRepository
    {
        public ReassignmentRuleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ReassignmentRule>> GetActiveRulesAsync()
        {
            return await _dbSet
                .Where(r => r.IsActive)
                .OrderByDescending(r => r.Priority)
                .ToListAsync();
        }

        public async Task<IEnumerable<ReassignmentRule>> GetRulesByResourceTypeAsync(int resourceTypeId)
        {
            return await _dbSet
                .Where(r => r.ResourceTypeId == resourceTypeId && r.IsActive)
                .OrderByDescending(r => r.Priority)
                .ToListAsync();
        }

        public async Task<IEnumerable<ReassignmentRule>> GetRulesByPriorityAsync(int minimumPriority)
        {
            return await _dbSet
                .Where(r => r.Priority >= minimumPriority && r.IsActive)
                .OrderByDescending(r => r.Priority)
                .ToListAsync();
        }
    }
}