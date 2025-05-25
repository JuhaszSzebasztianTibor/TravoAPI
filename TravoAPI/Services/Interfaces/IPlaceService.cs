using TravoAPI.Dtos.Planner;

namespace TravoAPI.Services.Interfaces
{
    public interface IPlaceService
    {
        Task<PlaceDto> AddPlaceAsync(string userId, PlaceDto dto);
        Task<IEnumerable<PlaceDto>> GetByDayPlanAsync(string userId, int dayPlanId);
        Task<IEnumerable<PlaceDto>> GetByTripAsync(string userId, int tripId);
        Task<bool> DeleteAsync(string userId, int placeId);
    }
}
