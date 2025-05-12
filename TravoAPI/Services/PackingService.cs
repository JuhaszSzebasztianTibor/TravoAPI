using TravoAPI.Data.Interfaces;
using TravoAPI.Dtos.Packing;
using TravoAPI.Models;
using TravoAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravoAPI.Services
{
    public class PackingService : IPackingService
    {
        private readonly IPackingRepository _repo;

        public PackingService(IPackingRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PackingListDto>> GetAllListsAsync(string userId)
        {
            var lists = await _repo.GetByUserAsync(userId);
            return lists.Select(MapToDto);
        }

        public async Task<PackingListDto?> GetListByIdAsync(int id, string userId)
        {
            // Only need the list metadata & items DTO'd; no need for full items collection load here
            var list = await _repo.GetByIdAsync(id);
            if (list == null || (list.UserId != "SYSTEM" && list.UserId != userId))
                return null;
            return MapToDto(list);
        }

        public async Task<PackingListDto> CreateListAsync(PackingListDto dto, string userId)
        {
            var entity = new PackingList
            {
                UserId = userId,
                Name = dto.Name,
                Category = dto.Category,
                Items = dto.Items.Select(i => new PackingItem
                {
                    Name = i.Name,
                    Quantity = i.Quantity.Value,
                    IsChecked = i.IsChecked ?? false
                }).ToList()
            };
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        public async Task<bool> UpdateListAsync(PackingListDto dto, string userId)
        {
            var entity = await _repo.GetByIdWithItemsAsync(dto.Id);
            if (entity == null || entity.UserId != userId)
                return false;

            entity.Name = dto.Name;
            entity.Category = dto.Category;

            // Replace items wholesale
            entity.Items.Clear();
            entity.Items.AddRange(dto.Items.Select(i => new PackingItem
            {
                Name = i.Name,
                Quantity = i.Quantity.Value,
                IsChecked = i.IsChecked ?? false
            }));

            _repo.Update(entity);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteListAsync(int id, string userId)
        {
            var entity = await _repo.GetByIdWithItemsAsync(id);
            if (entity == null || entity.UserId != userId)
                return false;

            _repo.Delete(entity);
            return await _repo.SaveChangesAsync();
        }

        public async Task<PackingItemDto?> AddItemToListAsync(int listId, PackingItemDto dto, string userId)
        {
            var list = await _repo.GetByIdWithItemsAsync(listId);
            if (list == null || (list.UserId != "SYSTEM" && list.UserId != userId))
                return null;

            var entity = new PackingItem
            {
                Name = dto.Name,
                Quantity = dto.Quantity ?? 1,
                IsChecked = dto.IsChecked ?? false
            };
            list.Items.Add(entity);
            await _repo.SaveChangesAsync();

            // Now entity.Id is set by EF
            return new PackingItemDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Quantity = entity.Quantity,
                IsChecked = entity.IsChecked
            };
        }


        public async Task<bool> UpdateItemAsync(int listId, int itemId, PackingItemDto itemDto, string userId)
        {
            // Load with Items to ensure Items is populated
            var list = await _repo.GetByIdWithItemsAsync(listId);
            if (list == null || (list.UserId != userId && list.UserId != "SYSTEM"))
                return false;

            var item = list.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                return false;

            // Apply patch fields
            item.Name = itemDto.Name ?? item.Name;
            item.Quantity = itemDto.Quantity.HasValue ? itemDto.Quantity.Value : item.Quantity;
            item.IsChecked = itemDto.IsChecked.HasValue ? itemDto.IsChecked.Value : item.IsChecked;

            _repo.Update(list);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> RemoveItemAsync(int listId, int itemId, string userId)
        {
            var list = await _repo.GetByIdWithItemsAsync(listId);
            if (list == null || (list.UserId != userId && list.UserId != "SYSTEM"))
                return false;

            var item = list.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                return false;

            list.Items.Remove(item);
            return await _repo.SaveChangesAsync();
        }

        private static PackingListDto MapToDto(PackingList pl) => new PackingListDto
        {
            Id = pl.Id,
            Name = pl.Name,
            Category = pl.Category,
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
