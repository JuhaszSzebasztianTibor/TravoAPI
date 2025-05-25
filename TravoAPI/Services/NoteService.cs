using AutoMapper;
using TravoAPI.Data.Interfaces;
using TravoAPI.Dtos.Planner;
using TravoAPI.Models;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Services
{
    public class NoteService : INoteService
    {
        private readonly IGenericRepository<Note> _repo;
        private readonly IMapper _mapper;

        public NoteService(IGenericRepository<Note> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<NoteDto> AddNoteAsync(int placeId, NoteDto dto)
        {
            var entity = _mapper.Map<Note>(dto);
            entity.PlaceId = placeId;
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return _mapper.Map<NoteDto>(entity);
        }

        public async Task<IEnumerable<NoteDto>> GetByPlaceAsync(int placeId)
        {
            var entities = await _repo.FindAsync(n => n.PlaceId == placeId);
            return _mapper.Map<IEnumerable<NoteDto>>(entities);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            _repo.Delete(entity);
            return await _repo.SaveChangesAsync();
        }
    }
}
