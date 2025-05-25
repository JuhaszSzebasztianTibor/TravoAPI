using TravoAPI.Models;

namespace TravoAPI.Data.Interfaces
{
    public interface IBudgetRepository : IGenericRepository<Budget>
    {
        /// <summary>
        /// Retrieves all budgets belonging to the given user, ordered by Day descending.
        /// </summary>
        Task<IEnumerable<Budget>> GetByUserAsync(string userId);
    }
}
