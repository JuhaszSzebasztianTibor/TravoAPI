using Microsoft.EntityFrameworkCore;
using TravoAPI.Data;
using TravoAPI.Interfaces;
using TravoAPI.Models;

namespace TravoAPI.Repositories
{
    public class TripRepositories : ITripService
    {
        private readonly ApplicationDBContext _context;

        public TripRepositories(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Trip> GetByIdAsync(int id)
        {
            return await _context.Trips.FindAsync(id);
        }

        public async Task<List<Trip>> GetAllAsync()
        {
            return await _context.Trips.ToListAsync();
        }

        public async Task<Trip> AddAsync(Trip trip)
        {
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();
            return trip;
        }

        public async Task<Trip> UpdateAsync(Trip trip)
        {
            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();
            return trip;
        }

        public async Task DeleteAsync(Trip trip)
        {
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
        }

    }
}
