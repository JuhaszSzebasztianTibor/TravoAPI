using Microsoft.EntityFrameworkCore;
using TravoAPI.Data;
using TravoAPI.Models;
using TravoAPI.Repositories.Interfaces;

namespace TravoAPI.Repositories
{
    public class TripRepository : GenericRepository<Trip>, ITripRepository
    {

        private readonly ApplicationDBContext _context;

        public TripRepository(ApplicationDBContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trip>> GetByUserAsync(string userId)
        {
            return await _context.Trips
                                 .Where(t => t.UserId == userId)
                                 .ToListAsync();
        }
    }
}
