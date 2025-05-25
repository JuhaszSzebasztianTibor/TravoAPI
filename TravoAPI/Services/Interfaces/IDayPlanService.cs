using TravoAPI.Dtos.Planner;

namespace TravoAPI.Services.Interfaces
{
    public interface IDayPlanService
    {
        Task<DayPlanDto> AddDayPlanAsync(DayPlanDto dto);
        Task<IEnumerable<DayPlanDto>> GetByTripIdAsync(int tripId);
        Task<DayPlanDto> GetByIdAsync(int id);

        Task<bool> DeleteAsync(int id);
    }
}
