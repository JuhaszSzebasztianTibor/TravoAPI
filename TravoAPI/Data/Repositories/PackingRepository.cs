using Microsoft.EntityFrameworkCore;
using TravoAPI.Data.Interfaces;
using TravoAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravoAPI.Data.Repositories
{
    public class PackingRepository : GenericRepository<PackingList>, IPackingRepository
    {
        private readonly ApplicationDBContext _ctx;

        public PackingRepository(ApplicationDBContext ctx)
            : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<PackingList>> GetByUserAsync(string userId)
            => await _ctx.PackingLists
                         .Include(pl => pl.Items)
                         .Where(pl => pl.UserId == "SYSTEM" || pl.UserId == userId)
                         .ToListAsync();

        public async Task<PackingList?> GetByIdAsync(int id)
        {
            // Inherited GenericRepository.FindAsync behavior
            return await base.GetByIdAsync(id);
        }

        public async Task<PackingList?> GetByIdWithItemsAsync(int id)
            => await _ctx.PackingLists
                         .Include(pl => pl.Items)
                         .FirstOrDefaultAsync(pl => pl.Id == id);
    }
}
