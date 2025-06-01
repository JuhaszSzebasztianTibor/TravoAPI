// Data/Repositories/GenericRepository.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TravoAPI.Data;
using TravoAPI.Repositories.Interfaces;

namespace TravoAPI.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected readonly ApplicationDBContext _context;
        protected readonly DbSet<TEntity> _db;

        public GenericRepository(ApplicationDBContext context)
        {
            _context = context;
            _db = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
            => await _db.AddAsync(entity);

        public void Delete(TEntity entity)
            => _db.Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _db.ToListAsync();

        public async Task<TEntity> GetByIdAsync(int id)
            => await _db.FindAsync(id);

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
            => await _db.Where(predicate).ToListAsync();
        public async Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes
        )
        {
            IQueryable<TEntity> query = _db;
            foreach (var include in includes)
                query = query.Include(include);
            return await query.Where(predicate).ToListAsync();
        }

        public void Update(TEntity entity)
            => _db.Update(entity);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
