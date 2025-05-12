using TravoAPI.Data.Interfaces;
using TravoAPI.Dtos.CreateTrip;
using TravoAPI.Models;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Services
{
    public class TripService : ITripService
    {
        private readonly IGenericRepository<Trip> _repo;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<TripService> _logger;

        public TripService(
            IGenericRepository<Trip> repo,
            IWebHostEnvironment env,
            ILogger<TripService> logger)
        {
            _repo = repo;
            _env = env;
            _logger = logger;
        }

        public async Task<Trip> CreateTripAsync(string userId, TripDto dto)
        {
            var imagePath = await SaveImageAsync(dto);
            var trip = new Trip
            {
                TripName = dto.TripName,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Image = imagePath,
                UserId = userId
            };
            await _repo.AddAsync(trip);
            await _repo.SaveChangesAsync();
            return trip;
        }

        public async Task<Trip> GetTripAsync(int id) =>
            await _repo.GetByIdAsync(id);

        public async Task<IEnumerable<Trip>> GetAllTripsAsync() =>
            await _repo.GetAllAsync();

        public async Task<Trip> UpdateTripAsync(string userId, int id, TripDto dto)
        {
            var trip = await _repo.GetByIdAsync(id);
            if (trip == null) return null;
            if (trip.UserId != userId) throw new UnauthorizedAccessException();

            trip.Image = await SaveImageAsync(dto, trip.Image);
            trip.TripName = dto.TripName;
            trip.Description = dto.Description;
            trip.StartDate = dto.StartDate;
            trip.EndDate = dto.EndDate;

            _repo.Update(trip);
            await _repo.SaveChangesAsync();
            return trip;
        }

        public async Task<bool> DeleteTripAsync(string userId, int id)
        {
            var trip = await _repo.GetByIdAsync(id);
            if (trip == null) return false;
            if (trip.UserId != userId) throw new UnauthorizedAccessException();

            if (!string.IsNullOrEmpty(trip.Image) && trip.Image.StartsWith("Uploads/"))
            {
                var full = Path.Combine(_env.WebRootPath, trip.Image.TrimStart('/'));
                if (File.Exists(full)) File.Delete(full);
            }

            _repo.Delete(trip);
            return await _repo.SaveChangesAsync();
        }

        public string GetAbsoluteUrl(string path) => path;

        private async Task<string> SaveImageAsync(TripDto dto, string existingPath = null)
        {
            if (dto.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(existingPath) && existingPath.StartsWith("Uploads/"))
                {
                    var old = Path.Combine(_env.WebRootPath, existingPath.TrimStart('/'));
                    if (File.Exists(old)) File.Delete(old);
                }

                var folder = Path.Combine(_env.WebRootPath, "Uploads");
                Directory.CreateDirectory(folder);
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
                var full = Path.Combine(folder, fileName);

                using var stream = new FileStream(full, FileMode.Create);
                await dto.ImageFile.CopyToAsync(stream);

                return $"Uploads/{fileName}";
            }

            return string.IsNullOrWhiteSpace(dto.ImageUrl) ? existingPath : dto.ImageUrl;
        }
    }
}
