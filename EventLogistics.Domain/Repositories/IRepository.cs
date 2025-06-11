using EventLogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Domain.Repositories
{
    // Generic Repository Interface
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
        Task<Guid> FindAsync(Func<object, bool> value);
        Task GetById(Guid eventId);
    }
}