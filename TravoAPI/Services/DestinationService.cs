using AutoMapper;
using TravoAPI.Data.Interfaces;
using TravoAPI.Dtos.Planner;
using TravoAPI.Models;
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

        public async Task<IEnumerable<DestinationDto>> GetAllAsync(int tripId) =>
            (await _repo.GetByTripAsync(tripId))
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
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            _repo.Delete(entity);
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