using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;

namespace EventLogistics.Infrastructure.Repositories
{
    public class OrganizatorRepository : Repository<Organizator>, IOrganizatorRepository
    {
        public OrganizatorRepository(EventLogisticsDbContext context) : base(context)
        {
        }

        public async Task<List<Organizator>> GetByRoleAsync(string role)
        {
            return await Task.FromResult(_context.Set<Organizator>()
                .Where(o => o.Role == role)
                .ToList());
        }

        public async Task<Organizator?> GetByEmailAsync(string email)
        {
            return await Task.FromResult(_context.Set<Organizator>()
                .FirstOrDefault(o => o.Email == email));
        }
    }
}
