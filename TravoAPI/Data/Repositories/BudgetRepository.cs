// Data/Repositories/BudgetRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravoAPI.Data.Interfaces;
using TravoAPI.Models;

namespace TravoAPI.Data.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly DbSet<Budget> _dbSet;

        public BudgetRepository(ApplicationDBContext context)
        {
            _context = context;
            _dbSet = context.Set<Budget>();
        }

        public async Task AddAsync(Budget entity)
            => await _dbSet.AddAsync(entity);

        public void Update(Budget entity)
            => _dbSet.Update(entity);

        public void Delete(Budget entity)
            => _dbSet.Remove(entity);

        public async Task<Budget> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<Budget>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<bool> SaveChangesAsync()
            => (await _context.SaveChangesAsync()) > 0;

        public async Task<IEnumerable<Budget>> GetByUserAsync(string userId)
            => await _dbSet
                 .Where(b => b.UserId == userId)
                 .OrderByDescending(b => b.Day)
                 .ToListAsync();

        // simple filter-only FindAsync
        public async Task<IEnumerable<Budget>> FindAsync(
            Expression<Func<Budget, bool>> predicate
        )
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        // filter + eager-load includes overload
        public async Task<IEnumerable<Budget>> FindAsync(
            Expression<Func<Budget, bool>> predicate,
            params Expression<Func<Budget, object>>[] includes
        )
        {
            IQueryable<Budget> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.Where(predicate).ToListAsync();
        }
    }
}
