using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravoAPI.Data;
using TravoAPI.Dtos.Planner;
using TravoAPI.Models;
using TravoAPI.Repositories.Interfaces;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Services
{
    public class DayPlanService : IDayPlanService
    {
        private readonly ApplicationDBContext _context;
        private readonly IGenericRepository<DayPlan> _repo;
        private readonly IMapper _mapper;

        public DayPlanService(
            ApplicationDBContext context,
            IGenericRepository<DayPlan> repo,
            IMapper mapper
        )
        {
            _context = context;
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<DayPlanDto> AddDayPlanAsync(DayPlanDto dto)
        {
            // map incoming DTO → entity
            var entity = _mapper.Map<DayPlan>(dto);

            // ensure FKs are set
            entity.TripId = dto.TripId;
            entity.DestinationId = dto.DestinationId;
            entity.Date = dto.Date;
            entity.Location = dto.Location;

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            // map back entity → DTO (now that Id is generated)
            return _mapper.Map<DayPlanDto>(entity);
        }

        public async Task<IEnumerable<DayPlanDto>> GetByTripIdAsync(int tripId)
        {
            var entities = await _context.DayPlans
              .Where(dp => dp.TripId == tripId)
              .Include(dp => dp.Destination)
              .Include(dp => dp.Places)
                  .ThenInclude(p => p.Notes)
              .Include(dp => dp.Places)
              .ToListAsync();

            return _mapper.Map<IEnumerable<DayPlanDto>>(entities);
        }

        public async Task<DayPlanDto> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return _mapper.Map<DayPlanDto>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            _repo.Delete(entity);
            return await _repo.SaveChangesAsync();
        }
    }
}
