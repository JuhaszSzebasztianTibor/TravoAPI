// Controllers/TripsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TravoAPI.Dtos.CreateTrip;
using TravoAPI.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace TravoAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _service;

        public TripsController(ITripService service)
        {
            _service = service;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateTrip([FromForm] TripDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var trip = await _service.CreateTripAsync(userId, dto);
            return CreatedAtAction(nameof(GetTrip), new { id = trip.Id }, new
            {
                trip.Id,
                trip.TripName,
                trip.StartDate,
                trip.EndDate,
                trip.Description,
                Image = _service.GetAbsoluteUrl(trip.Image),
                trip.UserId
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip(int id)
        {
            var trip = await _service.GetTripAsync(id);
            if (trip == null) return NotFound();
            return Ok(trip);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trips = await _service.GetAllTripsAsync();
            var result = trips.Select(t => new
            {
                t.Id,
                t.TripName,
                t.StartDate,
                t.EndDate,
                t.Description,
                Image = _service.GetAbsoluteUrl(t.Image),
                t.UserId
            });
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateTrip(int id, [FromForm] TripDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updated = await _service.UpdateTripAsync(userId, id, dto);
            if (updated == null) return NotFound();
            return Ok(new
            {
                updated.Id,
                updated.TripName,
                updated.StartDate,
                updated.EndDate,
                updated.Description,
                Image = _service.GetAbsoluteUrl(updated.Image),
                updated.UserId
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _service.DeleteTripAsync(userId, id);
            return success ? NoContent() : NotFound();
        }
    }
}
