// Controllers/DayPlanController.cs
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
            Console.WriteLine($"[DayPlanController] Error getting day plans: {ex.Message}");
            return StatusCode(500, "Something went wrong while fetching day plans.");
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