// Services/PackingService.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravoAPI.Data.Interfaces;
using TravoAPI.Dtos.Packing;
using TravoAPI.Models;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Services
{
    public class PackingService : IPackingService
    {
        private readonly IPackingRepository _repo;
        public PackingService(IPackingRepository repo) => _repo = repo;

        public async Task<IEnumerable<PackingListDto>> GetAllListsAsync(string userId, int tripId)
            => (await _repo.GetByUserAndTripAsync(userId, tripId)).Select(MapToDto);

        public async Task<PackingListDto?> GetListByIdAsync(string userId, int tripId, int listId)
        {
            var e = await _repo.GetByIdWithItemsAsync(listId);
            if (e == null || e.UserId != userId || e.TripId != tripId) return null;
            return MapToDto(e);
        }

        public async Task<PackingListDto> CreateListAsync(string userId, int tripId, PackingListDto dto)
        {
            var e = new PackingList
            {
                UserId = userId,
                TripId = tripId,
                Name = dto.Name,
                PackingListIcon = dto.PackingListIcon,
                Items = dto.Items.Select(i => new PackingItem
                {
                    Name = i.Name,
                    Quantity = i.Quantity,
                    IsChecked = i.IsChecked
                }).ToList()
            };
            await _repo.AddAsync(e); await _repo.SaveChangesAsync();
            dto.Id = e.Id; return dto;
        }

        public async Task<bool> UpdateListAsync(string userId, int tripId, PackingListDto dto)
        {
            var e = await _repo.GetByIdWithItemsAsync(dto.Id);
            if (e == null || e.UserId != userId || e.TripId != tripId) return false;
            e.Name = dto.Name; e.PackingListIcon = dto.PackingListIcon;
            e.Items.Clear(); e.Items.AddRange(dto.Items.Select(i => new PackingItem
            {
                Name = i.Name,
                Quantity = i.Quantity,
                IsChecked = i.IsChecked
            }));
            _repo.Update(e); return await _repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteListAsync(string userId, int tripId, int listId)
        {
            var e = await _repo.GetByIdWithItemsAsync(listId);
            if (e == null || e.UserId != userId || e.TripId != tripId) return false;
            _repo.Delete(e); return await _repo.SaveChangesAsync();
        }

        public async Task<PackingItemDto> AddItemToListAsync(int listId, string userId, PackingItemDto dto)
        {
            var l = await _repo.GetByIdWithItemsAsync(listId);
            if (l == null || l.UserId != userId) return null!;
            var i = new PackingItem { Name = dto.Name, Quantity = dto.Quantity, IsChecked = dto.IsChecked };
            l.Items.Add(i); await _repo.SaveChangesAsync();
            dto.Id = i.Id; return dto;
        }

        public async Task<bool> UpdateItemAsync(int listId, int itemId, string userId, PackingItemDto dto)
        {
            var l = await _repo.GetByIdWithItemsAsync(listId);
            if (l == null || l.UserId != userId) return false;
            var i = l.Items.FirstOrDefault(x => x.Id == itemId);
            if (i == null) return false;
            i.Name = dto.Name; i.Quantity = dto.Quantity; i.IsChecked = dto.IsChecked;
            _repo.Update(l); return await _repo.SaveChangesAsync();
        }

        public async Task<bool> RemoveItemAsync(int listId, int itemId, string userId)
        {
            var l = await _repo.GetByIdWithItemsAsync(listId);
            if (l == null || l.UserId != userId) return false;
            var i = l.Items.FirstOrDefault(x => x.Id == itemId);
            if (i == null) return false; l.Items.Remove(i);
            return await _repo.SaveChangesAsync();
        }

        public async Task<Dictionary<string, List<PackingItemDto>>> GetTemplatesAsync()
            => (await _repo.GetByUserAsync("SYSTEM")).ToDictionary(
                pl => pl.Name,
                pl => pl.Items.Select(i => new PackingItemDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity,
                    IsChecked = i.IsChecked
                }).ToList()
            );

        private static PackingListDto MapToDto(PackingList pl)
            => new PackingListDto
            {
                Id = pl.Id,
                Name = pl.Name,
                PackingListIcon = pl.PackingListIcon,
                Items = pl.Items.Select(i => new PackingItemDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity,
                    IsChecked = i.IsChecked
                }).ToList()
            };
    }
}
