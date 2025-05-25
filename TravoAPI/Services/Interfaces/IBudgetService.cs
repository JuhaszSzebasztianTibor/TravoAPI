using TravoAPI.Dtos.Budget;
using TravoAPI.Models;

public interface IBudgetService
{
    Task<BudgetDto> CreateBudgetAsync(string userId, int tripId, BudgetCreateDto dto);
    Task<IEnumerable<BudgetDto>> GetBudgetsByTripAsync(string userId, int tripId);
    Task<BudgetDto> GetBudgetAsync(int id);
    Task<BudgetDto> UpdateBudgetAsync(string userId, int tripId, int id, BudgetCreateDto dto);
    Task<bool> DeleteBudgetAsync(string userId, int tripId, int id);
}
