// Data/Repositories/GenericRepository.cs
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravoAPI.Data;
using TravoAPI.Data.Interfaces;

namespace TravoAPI.Data.Repositories
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

        public void Update(TEntity entity)
            => _db.Update(entity);

        public async Task<bool> SaveChangesAsync()
            => (await _context.SaveChangesAsync()) > 0;
    }
}
