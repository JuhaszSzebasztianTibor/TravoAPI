using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TravoAPI.Dtos.Budget;
using TravoAPI.Models;
using TravoAPI.Repositories.Interfaces;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IGenericRepository<Budget> _repo;
        private readonly ITripService _tripService;
        private readonly IMapper _mapper;

        public BudgetService(
            IGenericRepository<Budget> repo,
            ITripService tripService,
            IMapper mapper)
        {
            _repo = repo;
            _tripService = tripService;
            _mapper = mapper;
        }

        public async Task<BudgetDto> CreateBudgetAsync(
            string userId, int tripId, BudgetCreateDto dto)
        {
            if (!await _tripService.ValidateTripOwnership(userId, tripId))
                throw new UnauthorizedAccessException("Trip access denied");

            var entity = _mapper.Map<Budget>(dto);
            entity.UserId = userId;
            entity.TripId = tripId;

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return _mapper.Map<BudgetDto>(entity);
        }

        public async Task<IEnumerable<BudgetDto>> GetBudgetsByTripAsync(
            string userId, int tripId)
        {
            if (!await _tripService.ValidateTripOwnership(userId, tripId))
                throw new UnauthorizedAccessException("Trip access denied");

            var all = await _repo.GetAllAsync();
            var filtered = all
                .Where(b => b.TripId == tripId && b.UserId == userId);

            return _mapper.Map<IEnumerable<BudgetDto>>(filtered);
        }

        public async Task<BudgetDto> GetBudgetAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return _mapper.Map<BudgetDto>(entity);
        }

        public async Task<BudgetDto> UpdateBudgetAsync(
            string userId, int tripId, int id, BudgetCreateDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            if (entity.TripId != tripId
             || entity.UserId != userId
             || !await _tripService.ValidateTripOwnership(userId, tripId))
                throw new UnauthorizedAccessException("Trip access denied");

            _mapper.Map(dto, entity);
            _repo.Update(entity);
            await _repo.SaveChangesAsync();

            return _mapper.Map<BudgetDto>(entity);
        }

        public async Task<bool> DeleteBudgetAsync(
            string userId, int tripId, int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            if (entity.TripId != tripId
             || entity.UserId != userId
             || !await _tripService.ValidateTripOwnership(userId, tripId))
                throw new UnauthorizedAccessException("Trip access denied");

            _repo.Delete(entity);
            return await _repo.SaveChangesAsync();
        }
    }
}
