using TravoAPI.Dtos.Packing;

namespace TravoAPI.Services.Interfaces
{
    public interface IPackingService
    {
        Task<IEnumerable<PackingListDto>> GetAllListsAsync(string userId);
        Task<PackingListDto?> GetListByIdAsync(int id, string userId);
        Task<PackingListDto> CreateListAsync(PackingListDto dto, string userId);
        Task<bool> UpdateListAsync(PackingListDto dto, string userId);
        Task<bool> DeleteListAsync(int id, string userId);
        Task<PackingItemDto> AddItemToListAsync(int listId, PackingItemDto item, string userId);
        Task<bool> UpdateItemAsync(int listId, int itemId, PackingItemDto item, string userId);
        Task<bool> RemoveItemAsync(int listId, int itemId, string userId);
    }
}
