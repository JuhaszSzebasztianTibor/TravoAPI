using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravoAPI.Dtos.CreateTrip;
using TravoAPI.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TripsController : ControllerBase
{
    private readonly ITripService _service;
    private readonly IMapper _mapper;

    public TripsController(ITripService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    // GET /api/trips
    [HttpGet]
    public async Task<IActionResult> GetMyTrips()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var trips = await _service.GetTripsByUserAsync(userId);

        var payload = trips.Select(t => new {
            id = t.Id,
            tripName = t.TripName,
            description = t.Description,
            startDate = t.StartDate,
            endDate = t.EndDate,
            imageUrl = t.Image != null && t.Image.StartsWith("Uploads/", StringComparison.OrdinalIgnoreCase)
                          ? _service.GetAbsoluteUrl(t.Image)
                          : t.Image ?? ""
        }).ToList();

        return Ok(payload);
    }

    // GET /api/trips/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTrip(int id)
    {
        var trip = await _service.GetTripAsync(id);
        if (trip == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (trip.UserId != userId) return Forbid();

        var dto = _mapper.Map<TripDto>(trip);
        dto.ImageUrl = trip.Image != null && trip.Image.StartsWith("Uploads/", StringComparison.OrdinalIgnoreCase)
                       ? _service.GetAbsoluteUrl(trip.Image)
                       : trip.Image;

        return Ok(dto);
    }

    // POST /api/trips
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateTrip([FromForm] TripDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var trip = await _service.CreateTripAsync(userId, dto);

        var result = _mapper.Map<TripDto>(trip);
        result.ImageUrl = trip.Image != null && trip.Image.StartsWith("Uploads/", StringComparison.OrdinalIgnoreCase)
                          ? _service.GetAbsoluteUrl(trip.Image)
                          : trip.Image;

        return CreatedAtAction(nameof(GetTrip), new { id = trip.Id }, result);
    }

    // PUT /api/trips/{id}
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateTrip(int id, [FromForm] TripDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var updated = await _service.UpdateTripAsync(userId, id, dto);
        if (updated == null) return NotFound();

        var result = _mapper.Map<TripDto>(updated);
        result.ImageUrl = updated.Image != null && updated.Image.StartsWith("Uploads/", StringComparison.OrdinalIgnoreCase)
                          ? _service.GetAbsoluteUrl(updated.Image)
                          : updated.Image;

        return Ok(result);
    }

    // DELETE /api/trips/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrip(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var success = await _service.DeleteTripAsync(userId, id);
        return success ? NoContent() : NotFound();
    }
}
