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

    [HttpGet]
    public async Task<IActionResult> GetMyTrips()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // 1) Fetch your Trip entities
        var trips = await _service.GetTripsByUserAsync(userId);
        var payload = trips.Select(t => new {
            id = t.Id,
            tripName = t.TripName,
            description = t.Description,
            startDate = t.StartDate,
            endDate = t.EndDate,
            imageUrl = _service.GetAbsoluteUrl(t.Image) ?? ""
        }).ToList();

        // 3) Return that list—guaranteed to include imageUrl on every object
        return Ok(payload);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTrip(int id)
    {
        var trip = await _service.GetTripAsync(id);
        if (trip == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (trip.UserId != userId) return Forbid();

        var dto = _mapper.Map<TripDto>(trip);
        dto.ImageUrl = _service.GetAbsoluteUrl(trip.Image);
        return Ok(dto);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateTrip([FromForm] TripDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var trip = await _service.CreateTripAsync(userId, dto);

        var result = _mapper.Map<TripDto>(trip);
        result.ImageUrl = _service.GetAbsoluteUrl(trip.Image);

        return CreatedAtAction(nameof(GetTrip), new { id = trip.Id }, result);
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateTrip(int id, [FromForm] TripDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var updated = await _service.UpdateTripAsync(userId, id, dto);
        if (updated == null) return NotFound();

        var result = _mapper.Map<TripDto>(updated);
        result.ImageUrl = _service.GetAbsoluteUrl(updated.Image);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrip(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var success = await _service.DeleteTripAsync(userId, id);
        return success ? NoContent() : NotFound();
    }
}

