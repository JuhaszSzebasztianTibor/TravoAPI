using TravoAPI.Data.Interfaces;
using TravoAPI.Dtos.Budget;
using TravoAPI.Models;
using TravoAPI.Models.Enums;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IGenericRepository<Budget> _repo;
        private readonly ITripService _tripService;

        public BudgetService(
            IGenericRepository<Budget> repo,
            ITripService tripService)
        {
            _repo = repo;
            _tripService = tripService;
        }

        public async Task<Budget> CreateBudgetAsync(string userId, int tripId, CreateBudgetDto dto)
        {
            // ensure the user owns the trip
            if (!await _tripService.ValidateTripOwnership(userId, tripId))
                throw new UnauthorizedAccessException("Trip access denied");

            var budget = new Budget
            {
                Day = dto.Day,
                Category = dto.Category,
                Name = dto.Name,
                Amount = dto.Amount,
                Status = Enum.Parse<BudgetStatus>(dto.Status),
                UserId = userId,
                TripId = tripId
            };

            await _repo.AddAsync(budget);
            await _repo.SaveChangesAsync();
            return budget;
        }

        public async Task<IEnumerable<Budget>> GetBudgetsByTripAsync(string userId, int tripId)
        {
            // ensure the user owns the trip
            if (!await _tripService.ValidateTripOwnership(userId, tripId))
                throw new UnauthorizedAccessException("Trip access denied");

            // fetch all budgets for this trip
            return (await _repo.GetAllAsync())
                .Where(b => b.TripId == tripId && b.UserId == userId);
        }

        public async Task<Budget> GetBudgetAsync(int id)
        {
            // raw fetch; ownership must be checked by the controller
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Budget> UpdateBudgetAsync(string userId, int tripId, int id, UpdateBudgetDto dto)
        {
            var budget = await _repo.GetByIdAsync(id);
            if (budget == null)
                return null;

            // check both trip scope and user ownership
            if (budget.TripId != tripId ||
                budget.UserId != userId ||
                !await _tripService.ValidateTripOwnership(userId, tripId))
                throw new UnauthorizedAccessException("Trip access denied");

            // apply updates
            budget.Day = dto.Day;
            budget.Category = dto.Category;
            budget.Name = dto.Name;
            budget.Amount = dto.Amount;
            budget.Status = Enum.Parse<BudgetStatus>(dto.Status);

            _repo.Update(budget);
            await _repo.SaveChangesAsync();
            return budget;
        }

        public async Task<bool> DeleteBudgetAsync(string userId, int tripId, int id)
        {
            var budget = await _repo.GetByIdAsync(id);
            if (budget == null)
                return false;

            // ensure it's the right trip and user
            if (budget.TripId != tripId ||
                budget.UserId != userId ||
                !await _tripService.ValidateTripOwnership(userId, tripId))
                throw new UnauthorizedAccessException("Trip access denied");

            _repo.Delete(budget);
            return await _repo.SaveChangesAsync();
        }
    }
}
