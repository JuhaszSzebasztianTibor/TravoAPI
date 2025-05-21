using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravoAPI.Dtos.Planner;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Controllers
{
    [ApiController]
    [Route("api/trips/{tripId}/[controller]")]
    [Authorize]
    public class DestinationsController : ControllerBase
    {
        private readonly IDestinationService _svc;
        public DestinationsController(IDestinationService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> GetAll(int tripId)
            => Ok(await _svc.GetAllAsync(tripId));

        [HttpPost]
        public async Task<IActionResult> Create(int tripId, [FromBody] DestinationDto dto)
        {
            var created = await _svc.CreateAsync(tripId, dto);
            return CreatedAtAction(nameof(GetAll), new { tripId }, created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int tripId, int id)
        {
            var ok = await _svc.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }

        // Partial update for nights
        [HttpPatch("{id}/nights")]
        public async Task<IActionResult> PatchNights(int tripId, int id, [FromBody] int nights)
        {
            if (nights < 1 || nights > 365)
                return BadRequest("Nights must be between 1 and 365.");

            var ok = await _svc.UpdateNightsAsync(tripId, id, nights);
            return ok ? NoContent() : NotFound();
        }
    }
}
