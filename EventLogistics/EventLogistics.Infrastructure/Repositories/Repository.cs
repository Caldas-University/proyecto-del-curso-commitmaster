using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Infrastructure.Repositories
{    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly EventLogisticsDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(EventLogisticsDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task<Guid> FindAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        public Task GetById(Guid eventId)
        {
            throw new NotImplementedException();
        }
    }
}