using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravoAPI.Dtos.Budget;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/trips/{tripId}/budgets")]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetService _service;
        public BudgetsController(IBudgetService service) => _service = service;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpPost]
        public async Task<IActionResult> CreateBudget(
            int tripId,
            [FromBody] BudgetCreateDto dto)
        {
            var result = await _service.CreateBudgetAsync(UserId, tripId, dto);
            return CreatedAtAction(nameof(GetBudget),
                new { tripId, id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetBudgets(int tripId)
        {
            var list = await _service.GetBudgetsByTripAsync(UserId, tripId);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBudget(int tripId, int id)
        {
            var dto = await _service.GetBudgetAsync(id);
            if (dto == null || dto.TripId != tripId) return NotFound();
            if (dto.UserId != UserId) return Forbid();
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudget(
            int tripId,
            int id,
            [FromBody] BudgetCreateDto dto)    
        {
            var updated = await _service.UpdateBudgetAsync(
                UserId, tripId, id, dto);
            return updated != null ? Ok(updated) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int tripId, int id)
        {
            var success = await _service.DeleteBudgetAsync(
                UserId, tripId, id);
            return success ? NoContent() : NotFound();
        }
    }
}
