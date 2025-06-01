// Data/Repositories/PackingRepository.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravoAPI.Data;
using TravoAPI.Models;
using TravoAPI.Repositories.Interfaces;

namespace TravoAPI.Repositories
{
    public class PackingRepository : IPackingRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly DbSet<PackingList> _dbSet;

        public PackingRepository(ApplicationDBContext context)
        {
            _context = context;
            _dbSet = context.Set<PackingList>();
        }

        public async Task<PackingList?> GetByIdWithItemsAsync(int id)
        {
            return await _dbSet
                .Include(pl => pl.Items)
                .FirstOrDefaultAsync(pl => pl.Id == id);
        }

        public async Task<IEnumerable<PackingList>> GetByUserAsync(string userId)
        {
            return await _dbSet
                .Where(pl => pl.UserId == userId)
                .Include(pl => pl.Items)
                .ToListAsync();
        }

        public async Task AddAsync(PackingList entity) => await _dbSet.AddAsync(entity);
        public void Update(PackingList entity) => _dbSet.Update(entity);
        public void Delete(PackingList entity) => _dbSet.Remove(entity);

        public async Task<IEnumerable<PackingList>> GetByUserAndTripAsync(string userId, int tripId)
            => await _dbSet.Where(pl => pl.UserId == userId && pl.TripId == tripId)
                           .Include(pl => pl.Items).ToListAsync();

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}

