// Controllers/PackingController.cs
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravoAPI.Dtos.Packing;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/trips/{tripId}/packing")]
    public class PackingController : ControllerBase
    {
        private readonly IPackingService _svc;
        private readonly ITripService _tripSvc;
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public PackingController(IPackingService svc, ITripService tripSvc)
        {
            _svc = svc;
            _tripSvc = tripSvc;
        }

        [AllowAnonymous]
        [HttpGet("templates")]
        public async Task<IActionResult> GetTemplates()
            => Ok(new { templates = await _svc.GetTemplatesAsync() });

        [HttpGet]
        public async Task<IActionResult> GetAll(int tripId)
        {
            if (!await _tripSvc.ValidateTripOwnership(UserId, tripId))
                return Forbid();

            return Ok(await _svc.GetAllListsAsync(UserId, tripId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int tripId, int id)
        {
            if (!await _tripSvc.ValidateTripOwnership(UserId, tripId))
                return Forbid();

            var d = await _svc.GetListByIdAsync(UserId, tripId, id);
            return d == null ? NotFound() : Ok(d);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int tripId, [FromBody] PackingListDto dto)
        {
            if (!await _tripSvc.ValidateTripOwnership(UserId, tripId))
                return Forbid();

            var created = await _svc.CreateListAsync(UserId, tripId, dto);

            // supply both tripId & id, so CreatedAtAction can match the [HttpGet("{id}")]
            return CreatedAtAction(
                nameof(Get),
                new { tripId = tripId, id = created.Id },
                created
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int tripId, int id, [FromBody] PackingListDto dto)
        {
            if (!await _tripSvc.ValidateTripOwnership(UserId, tripId))
                return Forbid();

            dto.Id = id;
            var ok = await _svc.UpdateListAsync(UserId, tripId, dto);
            return ok ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int tripId, int id)
        {
            if (!await _tripSvc.ValidateTripOwnership(UserId, tripId))
                return Forbid();

            var ok = await _svc.DeleteListAsync(UserId, tripId, id);
            return ok ? NoContent() : NotFound();
        }

        [HttpPost("{listId}/items")]
        public async Task<IActionResult> AddItem(int tripId, int listId, [FromBody] PackingItemDto dto)
        {
            if (!await _tripSvc.ValidateTripOwnership(UserId, tripId))
                return Forbid();

            var c = await _svc.AddItemToListAsync(listId, UserId, dto);
            return CreatedAtAction(
                nameof(UpdateItem),
                new { tripId = tripId, listId = listId, itemId = c.Id },
                c
            );
        }

        [HttpPatch("{listId}/items/{itemId}")]
        public async Task<IActionResult> UpdateItem(int tripId, int listId, int itemId, [FromBody] PackingItemDto dto)
        {
            if (!await _tripSvc.ValidateTripOwnership(UserId, tripId))
                return Forbid();

            var ok = await _svc.UpdateItemAsync(listId, itemId, UserId, dto);
            return ok ? Ok() : NotFound();
        }

        [HttpDelete("{listId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItem(int tripId, int listId, int itemId)
        {
            if (!await _tripSvc.ValidateTripOwnership(UserId, tripId))
                return Forbid();

            var ok = await _svc.RemoveItemAsync(listId, itemId, UserId);
            return ok ? NoContent() : NotFound();
        }

        [HttpGet("{listId}/items")]
        public async Task<IActionResult> GetItems(int tripId, int listId)
        {
            if (!await _tripSvc.ValidateTripOwnership(UserId, tripId))
                return Forbid();

            var d = await _svc.GetListByIdAsync(UserId, tripId, listId);
            return d == null ? NotFound() : Ok(d.Items);
        }
    }
}
