using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TravoAPI.Dtos.Packing;
using TravoAPI.Models;
using TravoAPI.Repositories.Interfaces;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Services
{
    public class PackingService : IPackingService
    {
        private readonly IPackingRepository _repo;
        private readonly IMapper _mapper;

        public PackingService(IPackingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PackingListDto>> GetAllListsAsync(string userId, int tripId)
        {
            var entities = await _repo.GetByUserAndTripAsync(userId, tripId);
            return _mapper.Map<IEnumerable<PackingListDto>>(entities);
        }

        public async Task<PackingListDto?> GetListByIdAsync(string userId, int tripId, int listId)
        {
            var entity = await _repo.GetByIdWithItemsAsync(listId);
            if (entity == null || entity.UserId != userId || entity.TripId != tripId)
                return null;

            return _mapper.Map<PackingListDto>(entity);
        }

        public async Task<PackingListDto> CreateListAsync(string userId, int tripId, PackingListDto dto)
        {
            var entity = _mapper.Map<PackingList>(dto);
            entity.UserId = userId;
            entity.TripId = tripId;

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return _mapper.Map<PackingListDto>(entity);
        }

        public async Task<bool> UpdateListAsync(string userId, int tripId, PackingListDto dto)
        {
            var entity = await _repo.GetByIdWithItemsAsync(dto.Id);
            if (entity == null || entity.UserId != userId || entity.TripId != tripId)
                return false;

            // Map incoming DTO onto existing entity (preserves Id, navigation props)
            _mapper.Map(dto, entity);
            entity.UserId = userId;
            entity.TripId = tripId;

            _repo.Update(entity);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteListAsync(string userId, int tripId, int listId)
        {
            var entity = await _repo.GetByIdWithItemsAsync(listId);
            if (entity == null || entity.UserId != userId || entity.TripId != tripId)
                return false;

            _repo.Delete(entity);
            return await _repo.SaveChangesAsync();
        }

        public async Task<PackingItemDto> AddItemToListAsync(int listId, string userId, PackingItemDto dto)
        {
            var entity = await _repo.GetByIdWithItemsAsync(listId);
            if (entity == null || entity.UserId != userId)
                return null!;

            var item = _mapper.Map<PackingItem>(dto);
            entity.Items.Add(item);
            await _repo.SaveChangesAsync();

            return _mapper.Map<PackingItemDto>(item);
        }

        public async Task<bool> UpdateItemAsync(int listId, int itemId, string userId, PackingItemDto dto)
        {
            var entity = await _repo.GetByIdWithItemsAsync(listId);
            if (entity == null || entity.UserId != userId)
                return false;

            var item = entity.Items.FirstOrDefault(x => x.Id == itemId);
            if (item == null) return false;

            _mapper.Map(dto, item);
            _repo.Update(entity);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> RemoveItemAsync(int listId, int itemId, string userId)
        {
            var entity = await _repo.GetByIdWithItemsAsync(listId);
            if (entity == null || entity.UserId != userId)
                return false;

            var item = entity.Items.FirstOrDefault(x => x.Id == itemId);
            if (item == null) return false;

            entity.Items.Remove(item);
            return await _repo.SaveChangesAsync();
        }

        public async Task<Dictionary<string, List<PackingItemDto>>> GetTemplatesAsync()
        {
            var templates = await _repo.GetByUserAsync("SYSTEM");
            return templates.ToDictionary(
                pl => pl.Name,
                pl => _mapper.Map<List<PackingItemDto>>(pl.Items)
            );
        }
    }
}
