using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravoAPI.Data.Interfaces;

namespace TravoAPI.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private readonly ApplicationDBContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDBContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);
        public void Delete(TEntity entity) => _dbSet.Remove(entity);
        public async Task<IEnumerable<TEntity>> GetAllAsync()
                                                      => await _dbSet.ToListAsync();
        public async Task<TEntity> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public void Update(TEntity entity) => _dbSet.Update(entity);
        public async Task<bool> SaveChangesAsync()
            => (await _context.SaveChangesAsync()) > 0;
    }
}
