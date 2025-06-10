using EventLogistics.Domain.Entities;

namespace EventLogistics.Domain.Repositories
{
    public interface IAssignmentRepository : IRepository<ResourceAssignment>    {
        // Puedes añadir métodos específicos si son necesarios
        Task<IEnumerable<ResourceAssignment>> GetByResourceIdAsync(Guid resourceId);
        Task<IEnumerable<ResourceAssignment>> GetByEventIdAsync(Guid eventId);
    }
}