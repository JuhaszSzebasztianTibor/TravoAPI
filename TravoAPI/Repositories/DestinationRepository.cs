using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravoAPI.Data;
using TravoAPI.Models;
using TravoAPI.Repositories.Interfaces;

namespace TravoAPI.Repositories
{
    public class DestinationRepository : GenericRepository<Destination>, IDestinationRepository
    {
        public DestinationRepository(ApplicationDBContext ctx) : base(ctx) { }

        public async Task<IEnumerable<Destination>> GetByTripAsync(int tripId)
        {
            return await _context.Destinations
                .Where(d => d.TripId == tripId)
                .Include(d => d.DayPlans)
                   .ThenInclude(dp => dp.Places)
                .ToListAsync();
        }

        public async Task<Destination> GetWithDayPlansAsync(int id)
        {
            return await _context.Destinations
                                 .Include(d => d.DayPlans)
                                    .ThenInclude(dp => dp.Places)
                                 .SingleOrDefaultAsync(d => d.Id == id);
        }

        public void DeleteDayPlans(IEnumerable<DayPlan> plans)
        {
            _context.DayPlans.RemoveRange(plans);
        }
    }
}
