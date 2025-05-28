using Microsoft.AspNetCore.Mvc;
using TravoAPI.Dtos.Planner;
using TravoAPI.Services.Interfaces;

[ApiController]
[Route("api/trips/{tripId}/dayplans")]
public class DayPlanController : ControllerBase
{
    private readonly IDayPlanService _svc;
    public DayPlanController(IDayPlanService svc) { _svc = svc; }

    [HttpPost]
    public async Task<IActionResult> Create(int tripId, [FromBody] DayPlanDto dto)
    {
        dto.TripId = tripId;
        if (dto.DestinationId <= 0)
            return BadRequest("You must supply a valid DestinationId.");

        var created = await _svc.AddDayPlanAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { tripId }, created);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int tripId)
    {
        try
        {
            var dps = await _svc.GetByTripIdAsync(tripId);
            return Ok(dps);
        }
        catch (Exception ex)
        {
            // In dev, return full details:
            return StatusCode(500, new
            {
                Error = ex.Message,
                Trace = ex.StackTrace
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int tripId, int id)
    {
        var dp = await _svc.GetByIdAsync(id);
        if (dp == null || dp.TripId != tripId) return NotFound();
        return Ok(dp);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int tripId, int id)
    {
        var success = await _svc.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
