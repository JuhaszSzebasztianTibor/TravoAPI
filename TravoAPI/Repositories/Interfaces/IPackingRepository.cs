// Data/Interfaces/IPackingRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using TravoAPI.Models;

namespace TravoAPI.Repositories.Interfaces
{
    public interface IPackingRepository
    {
        Task AddAsync(PackingList entity);
        void Update(PackingList entity);
        void Delete(PackingList entity);
        Task<PackingList?> GetByIdWithItemsAsync(int id);
        Task<IEnumerable<PackingList>> GetByUserAsync(string userId);
        Task<IEnumerable<PackingList>> GetByUserAndTripAsync(string userId, int tripId);
        Task<bool> SaveChangesAsync();
    }
}
