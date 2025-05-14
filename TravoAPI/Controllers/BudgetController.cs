using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravoAPI.Dtos.Budget;
using TravoAPI.Services.Interfaces;

[Authorize]
[ApiController]
[Route("api/trips/{tripId}/budgets")]
public class BudgetsController : ControllerBase
{
    private readonly IBudgetService _service;
    public BudgetsController(IBudgetService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> CreateBudget(int tripId, CreateBudgetDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var budget = await _service.CreateBudgetAsync(userId, tripId, dto);
        return CreatedAtAction(nameof(GetBudget),
            new { tripId, id = budget.Id }, budget);
    }

    [HttpGet]
    public async Task<IActionResult> GetBudgets(int tripId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var budgets = await _service.GetBudgetsByTripAsync(userId, tripId);
        return Ok(budgets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBudget(int tripId, int id)
    {
        var budget = await _service.GetBudgetAsync(id);
        if (budget == null || budget.TripId != tripId) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (budget.UserId != userId) return Forbid();

        return Ok(budget);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBudget(int tripId, int id, UpdateBudgetDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var updated = await _service.UpdateBudgetAsync(userId, tripId, id, dto);
        return updated != null ? Ok(updated) : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudget(int tripId, int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var success = await _service.DeleteBudgetAsync(userId, tripId, id);
        return success ? NoContent() : NotFound();
    }
}
