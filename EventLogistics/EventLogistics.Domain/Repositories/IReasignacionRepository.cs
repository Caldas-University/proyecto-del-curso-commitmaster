namespace EventLogistics.Domain.Repositories;

using EventLogistics.Domain.Entities;

public interface IReasignacionRepository
{
    Task AddAsync(Reasignacion reasignacion);
}
