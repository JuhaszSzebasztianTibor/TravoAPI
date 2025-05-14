using TravoAPI.Dtos.Budget;
using TravoAPI.Models;

public interface IBudgetService
{
    Task<Budget> CreateBudgetAsync(string userId, int tripId, CreateBudgetDto dto);
    Task<IEnumerable<Budget>> GetBudgetsByTripAsync(string userId, int tripId);
    Task<Budget> GetBudgetAsync(int id);
    Task<Budget> UpdateBudgetAsync(string userId, int tripId, int id, UpdateBudgetDto dto);
    Task<bool> DeleteBudgetAsync(string userId, int tripId, int id);
}
