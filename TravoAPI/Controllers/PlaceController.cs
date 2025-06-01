using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravoAPI.Dtos.Planner;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Controllers
{
    [ApiController]
    [Authorize]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceService _svc;
        public PlaceController(IPlaceService svc) { _svc = svc; }

        // GET /api/trips/{tripId}/places
        [HttpGet("api/trips/{tripId}/places")]
        public async Task<IActionResult> GetByTrip(int tripId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var places = await _svc.GetByTripAsync(userId, tripId);
            return Ok(places);
        }

        // GET /api/dayplans/{dayPlanId}/places
        [HttpGet("api/dayplans/{dayPlanId}/places")]
        public async Task<IActionResult> GetByDayPlan(int dayPlanId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var places = await _svc.GetByDayPlanAsync(userId, dayPlanId);
            return Ok(places);
        }


        // POST /api/dayplans/{dayPlanId}/places
        [HttpPost("api/dayplans/{dayPlanId}/places")]
        public async Task<IActionResult> Create(int dayPlanId, [FromBody] PlaceDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            dto.DayPlanId = dayPlanId;
            try
            {
                var created = await _svc.AddPlaceAsync(userId, dto);
                return CreatedAtAction(nameof(GetByTrip),
                                       new { tripId = created.TripId },
                                       created);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.InnerException?.Message ?? ex.Message, statusCode: 500);
            }
        }

        // DELETE /api/dayplans/{dayPlanId}/places/{id}
        [HttpDelete("api/dayplans/{dayPlanId}/places/{id}")]
        public async Task<IActionResult> Delete(int dayPlanId, int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _svc.DeleteAsync(userId, id);
            return success ? NoContent() : NotFound();
        }
    }
}
