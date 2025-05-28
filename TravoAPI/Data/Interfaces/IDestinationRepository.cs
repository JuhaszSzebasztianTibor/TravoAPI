using TravoAPI.Models;

namespace TravoAPI.Data.Interfaces
{
    public interface IDestinationRepository : IGenericRepository<Destination>
    {
        Task<IEnumerable<Destination>> GetByTripAsync(int tripId);

        Task<Destination> GetWithDayPlansAsync(int id);

        void DeleteDayPlans(IEnumerable<DayPlan> plans);
    }
}
