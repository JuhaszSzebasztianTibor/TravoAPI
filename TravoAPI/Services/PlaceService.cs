using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravoAPI.Data;
using TravoAPI.Dtos.Planner;
using TravoAPI.Models;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public PlaceService(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PlaceDto> AddPlaceAsync(string userId, PlaceDto dto)
        {
            // 1) Verify the DayPlan belongs to this user
            var dp = await _context.DayPlans
                        .Include(d => d.Trip)
                        .FirstOrDefaultAsync(d =>
                            d.Id == dto.DayPlanId &&
                            d.Trip.UserId == userId
                        );
            if (dp == null)
                throw new UnauthorizedAccessException("DayPlan not found or not yours.");

            // 2) Map & save
            var entity = _mapper.Map<Place>(dto);
            _context.Places.Add(entity);
            await _context.SaveChangesAsync();

            // 3) Reload DayPlan navigation so AutoMapper can read TripId
            await _context.Entry(entity)
                          .Reference(e => e.DayPlan)
                          .Query()
                          .Include(d => d.Trip)
                          .LoadAsync();

            // 4) Map back to DTO (TripId is now populated)
            var result = _mapper.Map<PlaceDto>(entity);
            return result;
        }

        public async Task<IEnumerable<PlaceDto>> GetByDayPlanAsync(string userId, int dayPlanId)
        {
            var entities = await _context.Places
                .AsNoTracking()
                .Include(p => p.Notes)
                .Include(p => p.DayPlan)
                   .ThenInclude(dp => dp.Trip)
                .Where(p =>
                    p.DayPlanId == dayPlanId &&
                    p.DayPlan.Trip.UserId == userId
                )
                .ToListAsync();

            return _mapper.Map<IEnumerable<PlaceDto>>(entities);
        }

        public async Task<IEnumerable<PlaceDto>> GetByTripAsync(string userId, int tripId)
        {
            var entities = await _context.Places
                .AsNoTracking()
                .Include(p => p.Notes)
                .Include(p => p.DayPlan)
                   .ThenInclude(dp => dp.Trip)
                .Where(p =>
                    p.DayPlan.TripId == tripId &&
                    p.DayPlan.Trip.UserId == userId
                )
                .ToListAsync();

            return _mapper.Map<IEnumerable<PlaceDto>>(entities);
        }

        public async Task<bool> DeleteAsync(string userId, int placeId)
        {
            var entity = await _context.Places
                .Include(p => p.DayPlan)
                   .ThenInclude(dp => dp.Trip)
                .FirstOrDefaultAsync(p =>
                    p.Id == placeId &&
                    p.DayPlan.Trip.UserId == userId
                );
            if (entity == null) return false;

            _context.Places.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
