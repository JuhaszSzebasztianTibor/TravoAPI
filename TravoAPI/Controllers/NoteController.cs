// Controllers/NoteController.cs
using Microsoft.AspNetCore.Mvc;
using TravoAPI.Dtos.Planner;
using TravoAPI.Services.Interfaces;

[ApiController]
[Route("api/places/{placeId}/notes")]
public class NoteController : ControllerBase
{
    private readonly INoteService _svc;
    public NoteController(INoteService svc) { _svc = svc; }

    [HttpGet]
    public async Task<IActionResult> GetAll(int placeId)
    {
        var notes = await _svc.GetByPlaceAsync(placeId);
        return Ok(notes);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int placeId, [FromBody] NoteDto dto)
    {
        var created = await _svc.AddNoteAsync(placeId, dto);
        return CreatedAtAction(nameof(GetAll), new { placeId }, created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int placeId, int id)
    {
        var success = await _svc.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}