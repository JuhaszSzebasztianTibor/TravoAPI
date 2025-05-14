// Services/Interfaces/IPackingService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using TravoAPI.Dtos.Packing;

namespace TravoAPI.Services.Interfaces
{
    public interface IPackingService
    {
        Task<IEnumerable<PackingListDto>> GetAllListsAsync(string userId, int tripId);
        Task<PackingListDto?> GetListByIdAsync(string userId, int tripId, int listId);
        Task<PackingListDto> CreateListAsync(string userId, int tripId, PackingListDto dto);
        Task<bool> UpdateListAsync(string userId, int tripId, PackingListDto dto);
        Task<bool> DeleteListAsync(string userId, int tripId, int listId);
        Task<PackingItemDto> AddItemToListAsync(int listId, string userId, PackingItemDto dto);
        Task<bool> UpdateItemAsync(int listId, int itemId, string userId, PackingItemDto dto);
        Task<bool> RemoveItemAsync(int listId, int itemId, string userId);
        Task<Dictionary<string, List<PackingItemDto>>> GetTemplatesAsync();
    }
}
