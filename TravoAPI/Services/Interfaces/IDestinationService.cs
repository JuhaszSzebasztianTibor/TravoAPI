using TravoAPI.Dtos.Planner;

namespace TravoAPI.Services.Interfaces
{
    public interface IDestinationService
    {
        Task<IEnumerable<DestinationDto>> GetAllAsync(int tripId);
        Task<DestinationDto> CreateAsync(int tripId, DestinationDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateNightsAsync(int tripId, int id, int nights);
    }
}
