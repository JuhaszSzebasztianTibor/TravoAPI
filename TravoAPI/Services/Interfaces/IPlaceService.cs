using TravoAPI.Dtos.Planner;

namespace TravoAPI.Services.Interfaces
{
    public interface IPlaceService
    {
        Task<IEnumerable<PlaceDto>> GetByTripAsync(string userId, int tripId);

        Task<IEnumerable<PlaceDto>> GetByDayPlanAsync(string userId, int dayPlanId);
        Task<PlaceDto> AddPlaceAsync(string userId, PlaceDto dto);

        Task<bool> DeleteAsync(string userId, int placeId);
    }
}
