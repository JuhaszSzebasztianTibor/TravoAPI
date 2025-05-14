using TravoAPI.Dtos.CreateTrip;
using TravoAPI.Models;

namespace TravoAPI.Services.Interfaces
{
    public interface ITripService
    {
        Task<Trip> CreateTripAsync(string userId, TripDto dto);
        Task<Trip> GetTripAsync(int id);
        Task<IEnumerable<Trip>> GetAllTripsAsync();
        Task<Trip> UpdateTripAsync(string userId, int id, TripDto dto);
        Task<bool> DeleteTripAsync(string userId, int id);
        string GetAbsoluteUrl(string relativePath);

        Task<IEnumerable<Trip>> GetTripsByUserAsync(string userId);

        public Task<bool> ValidateTripOwnership(string userId, int tripId);
    }
}
