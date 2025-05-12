using TravoAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravoAPI.Data.Interfaces
{
    public interface IPackingRepository
    {
        Task AddAsync(PackingList entity);
        void Delete(PackingList entity);

        /// <summary>
        /// Loads just the PackingList (no navigation properties).
        /// </summary>
        Task<PackingList?> GetByIdAsync(int id);

        /// <summary>
        /// Loads the PackingList together with its Items.
        /// </summary>
        Task<PackingList?> GetByIdWithItemsAsync(int id);

        Task<IEnumerable<PackingList>> GetByUserAsync(string userId);
        void Update(PackingList entity);
        Task<bool> SaveChangesAsync();
    }
}
