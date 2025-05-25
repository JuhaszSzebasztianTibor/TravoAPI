using TravoAPI.Dtos.Planner;

namespace TravoAPI.Services.Interfaces
{
    public interface INoteService
    {
        Task<NoteDto> AddNoteAsync(int placeId, NoteDto dto);
        Task<IEnumerable<NoteDto>> GetByPlaceAsync(int placeId);
        Task<bool> DeleteAsync(int id);
    }
}
