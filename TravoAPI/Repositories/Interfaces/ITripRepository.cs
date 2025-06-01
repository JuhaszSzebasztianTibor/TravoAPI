using TravoAPI.Models;

namespace TravoAPI.Repositories.Interfaces
{
    public interface ITripRepository : IGenericRepository<Trip>
    {
        Task<IEnumerable<Trip>> GetByUserAsync(string userId);
    }
}
