using TravoAPI.Models;

namespace TravoAPI.Data.Interfaces
{
    public interface ITripRepository : IGenericRepository<Trip>
    {
        Task<IEnumerable<Trip>> GetByUserAsync(string userId);
    }
}
