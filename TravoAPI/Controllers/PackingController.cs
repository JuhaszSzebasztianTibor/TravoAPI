using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravoAPI.Dtos.Packing;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackingController : ControllerBase
    {
        private readonly IPackingService _service;
        public PackingController(IPackingService service) => _service = service;

        private string? GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(userId) ? null : userId;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId() ?? "";
            return Ok(await _service.GetAllListsAsync(userId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _service.GetListByIdAsync(id, userId);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PackingListDto dto)
        {
            // 1. Retrieve and guard userId
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            // 2. Call the service
            var createdDto = await _service.CreateListAsync(dto, userId);
            if (createdDto == null)
                return BadRequest();

            // 3. Return 201 with Location header
            return CreatedAtAction(
                actionName: nameof(Get),
                routeValues: new { id = createdDto.Id },
                value: createdDto
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PackingListDto dto)
        {
            if (dto.Id != id) return BadRequest();

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var success = await _service.UpdateListAsync(dto, userId);
            return success ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var success = await _service.DeleteListAsync(id, userId);
            return success ? Ok() : NotFound();
        }

        [HttpPost("{listId}/items")]
        public async Task<IActionResult> AddItem(int listId, [FromBody] PackingItemDto itemDto)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            // Have service return the DTO of the newly created item
            var created = await _service.AddItemToListAsync(listId, itemDto, userId);
            if (created == null)
                return BadRequest();

            // 201 → /api/Packing/{listId}/items/{created.Id}
            return CreatedAtAction(
              actionName: nameof(UpdateItem),
              routeValues: new { listId, itemId = created.Id },
              value: created
            );
        }

        [HttpPatch("{listId}/items/{itemId}")]
        public async Task<IActionResult> UpdateItem(int listId, int itemId, [FromBody] PackingItemDto itemDto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            Console.WriteLine($"PATCH Request: list={listId}, item={itemId}");

            try
            {
                var success = await _service.UpdateItemAsync(listId, itemId, itemDto, userId);
                return success ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PATCH Error: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpDelete("{listId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItem(int listId, int itemId)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            Console.WriteLine($"DELETE Request: list={listId}, item={itemId}, user={userId}");
            try
            {
                var success = await _service.RemoveItemAsync(listId, itemId, userId);
                return success ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DELETE Error: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet("{listId}/items")]
        public async Task<IActionResult> GetListItems(int listId)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var list = await _service.GetListByIdAsync(listId, userId);
            return list is null ? NotFound() : Ok(list.Items);
        }
    }
}
