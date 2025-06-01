using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravoAPI.Dtos.Planner;
using TravoAPI.Models;
using TravoAPI.Repositories.Interfaces;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly IDestinationRepository _repo;
        private readonly IMapper _mapper;

        public DestinationService(IDestinationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DestinationDto>> GetAllAsync(int tripId)
            => (await _repo.GetByTripAsync(tripId))
                   .Select(d => _mapper.Map<DestinationDto>(d));

        public async Task<DestinationDto> CreateAsync(int tripId, DestinationDto dto)
        {
            var model = _mapper.Map<Destination>(dto);
            model.TripId = tripId;
            await _repo.AddAsync(model);
            await _repo.SaveChangesAsync();
            return _mapper.Map<DestinationDto>(model);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // 1) Load destination with its DayPlans (and Places)
            var dest = await _repo.GetWithDayPlansAsync(id);
            if (dest == null) return false;

            // 2) Delete all its DayPlans (their Places cascade‐delete)
            _repo.DeleteDayPlans(dest.DayPlans);

            // 3) Delete the destination
            _repo.Delete(dest);

            // 4) Commit all deletes in one SaveChanges call
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> UpdateNightsAsync(int tripId, int id, int nights)
        {
            var dest = await _repo.GetByIdAsync(id);
            if (dest == null || dest.TripId != tripId) return false;
            dest.Nights = nights;
            _repo.Update(dest);
            return await _repo.SaveChangesAsync();
        }
    }
}
