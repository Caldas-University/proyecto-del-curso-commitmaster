namespace EventLogistics.Infrastructure.Repositories;

using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

public class ReasignacionRepository : IReasignacionRepository
{
    private readonly EventLogisticsDbContext _dbContext;

    public ReasignacionRepository(EventLogisticsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Reasignacion reasignacion)
    {
        await _dbContext.Reasignaciones.AddAsync(reasignacion);
        await _dbContext.SaveChangesAsync();
    }
}
