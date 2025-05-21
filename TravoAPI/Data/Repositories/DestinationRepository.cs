using Microsoft.EntityFrameworkCore;
using TravoAPI.Data.Interfaces;
using TravoAPI.Models;

namespace TravoAPI.Data.Repositories
{
    public class DestinationRepository : GenericRepository<Destination>, IDestinationRepository
    {
        public DestinationRepository(ApplicationDBContext ctx) : base(ctx) { }
        public async Task<IEnumerable<Destination>> GetByTripAsync(int tripId)
            => await _context.Set<Destination>().Where(d => d.TripId == tripId).ToListAsync();
    }
}
