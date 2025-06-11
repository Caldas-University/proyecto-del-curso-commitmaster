using EventLogistics.Domain.Entities;

namespace EventLogistics.Domain.Repositories
{
    public interface IOrganizatorRepository : IRepository<Organizator>
    {
        Task<List<Organizator>> GetByRoleAsync(string role);
        Task<Organizator?> GetByEmailAsync(string email);
    }
}
