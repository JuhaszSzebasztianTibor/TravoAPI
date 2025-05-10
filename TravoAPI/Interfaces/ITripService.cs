using TravoAPI.Models;

namespace TravoAPI.Interfaces
{
    public interface ITripService
    {
        Task<Trip> GetByIdAsync(int id);
        Task<List<Trip>> GetAllAsync();
        Task<Trip> AddAsync(Trip trip);
        Task<Trip> UpdateAsync(Trip trip);
        Task DeleteAsync(Trip trip);
    }
}
