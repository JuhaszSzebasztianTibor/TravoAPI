// Services/DayPlanService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravoAPI.Data;
using TravoAPI.Data.Interfaces;
using TravoAPI.Dtos.Planner;
using TravoAPI.Models;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Services
{
    public class DayPlanService : IDayPlanService
    {
        private readonly ApplicationDBContext _context;
        private readonly IGenericRepository<DayPlan> _repo;     // for Create/Delete/etc.
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
            var entity = _mapper.Map<DayPlan>(dto);
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return _mapper.Map<DayPlanDto>(entity);
        }

        public async Task<IEnumerable<DayPlanDto>> GetByTripIdAsync(int tripId)
        {
            try
            {
                // directly use the DbContext to include navigations:
                var entities = await _context.DayPlans
                    .AsNoTracking()
                    .Where(dp => dp.TripId == tripId)
                    .Include(dp => dp.Places)
                       .ThenInclude(p => p.Notes)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<DayPlanDto>>(entities);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 ERROR in GetByTripIdAsync: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
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
