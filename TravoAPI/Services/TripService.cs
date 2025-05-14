using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TravoAPI.Data;
using TravoAPI.Data.Interfaces;
using TravoAPI.Dtos.CreateTrip;
using TravoAPI.Models;
using TravoAPI.Models.Enums;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Services
{
    public class TripService : ITripService
    {
        private readonly IGenericRepository<Trip> _repo;
        private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<TripService> _logger;

        public TripService(
            IGenericRepository<Trip> repo,
            ApplicationDBContext dbContext,
            IWebHostEnvironment env,
            ILogger<TripService> logger)
        {
            _repo = repo;
            _dbContext = dbContext;
            _env = env;
            _logger = logger;
        }

        public async Task<Trip> CreateTripAsync(string userId, TripDto dto)
        {
            // 1) Save image file or URL
            var imagePath = await SaveImageAsync(dto);

            // 2) Create the Trip record
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

            // 3) Seed SYSTEM packing‐list templates into this user’s trip
            var systemLists = await _dbContext.PackingLists
                .Include(pl => pl.Items)
                .Where(pl => pl.UserId == "SYSTEM")
                .ToListAsync();

            foreach (var sys in systemLists)
            {
                var clone = new PackingList
                {
                    Name = sys.Name,
                    PackingListIcon = sys.PackingListIcon,
                    TripId = trip.Id,
                    UserId = userId,
                    Items = sys.Items.Select(i => new PackingItem
                    {
                        Name = i.Name,
                        Quantity = i.Quantity,
                        IsChecked = false      // start unchecked
                    }).ToList()
                };
                _dbContext.PackingLists.Add(clone);
            }

            await _dbContext.SaveChangesAsync();

            return trip;
        }

        public async Task<IEnumerable<Trip>> GetTripsByUserAsync(string userId)
        {
            if (_repo is ITripRepository repoByUser)
                return await repoByUser.GetByUserAsync(userId);

            var all = await _repo.GetAllAsync();
            return all.Where(t => t.UserId == userId);
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

            // remove uploaded image file
            if (!string.IsNullOrEmpty(trip.Image) && trip.Image.StartsWith("Uploads/"))
            {
                var full = Path.Combine(_env.WebRootPath, trip.Image.TrimStart('/'));
                if (File.Exists(full))
                    File.Delete(full);
            }

            _repo.Delete(trip);
            return await _repo.SaveChangesAsync();
        }

        public string GetAbsoluteUrl(string path) => path;

        private async Task<string?> SaveImageAsync(TripDto dto, string? existingPath = null)
        {
            if (dto.ImageFile != null)
            {
                // delete old file if needed
                if (!string.IsNullOrEmpty(existingPath) && existingPath.StartsWith("Uploads/"))
                {
                    var old = Path.Combine(_env.WebRootPath, existingPath.TrimStart('/'));
                    if (File.Exists(old))
                        File.Delete(old);
                }

                var folder = Path.Combine(_env.WebRootPath, "Uploads");
                Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
                var full = Path.Combine(folder, fileName);

                using var stream = new FileStream(full, FileMode.Create);
                await dto.ImageFile.CopyToAsync(stream);

                return $"Uploads/{fileName}";
            }

            // if no file, use URL or keep existing
            return string.IsNullOrWhiteSpace(dto.ImageUrl)
                ? existingPath
                : dto.ImageUrl;
        }

        public async Task<bool> ValidateTripOwnership(string userId, int tripId)
        {
            var trip = await _repo.GetByIdAsync(tripId);
            return trip?.UserId == userId;
        }
    }
}

