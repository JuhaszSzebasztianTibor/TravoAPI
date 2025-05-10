using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using TravoAPI.Data;
using TravoAPI.Dtos.CreateTrip;
using TravoAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.IO;

namespace TravoAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<TripsController> _logger;

        public TripsController(ApplicationDBContext context, IWebHostEnvironment environment, ILogger<TripsController> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateTrip([FromForm] TripDto createTripDto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // Get user ID from the JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User not authenticated. Cannot create trip.");
                return Unauthorized("User not authenticated.");
            }

            string imagePath;

            // Save uploaded file
            try
            {
                if (createTripDto.ImageFile != null)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "Uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid() + Path.GetExtension(createTripDto.ImageFile.FileName);
                    var fullPath = Path.Combine(uploadsFolder, fileName);

                    // Add file size validation (example: 5MB max)
                    if (createTripDto.ImageFile.Length > 5 * 1024 * 1024)
                    {
                        _logger.LogWarning("File size exceeds 5MB limit");
                        return BadRequest("File size must be less than 5MB");
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await createTripDto.ImageFile.CopyToAsync(stream);
                    }

                    imagePath = $"/Uploads/{fileName}";
                }
                else
                {
                    // Use the image URL if no file was uploaded
                    imagePath = createTripDto.ImageUrl!;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file: {ex}"); // Log full exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            // Create and save the trip
            var trip = new Trip
            {
                TripName = createTripDto.TripName,
                Description = createTripDto.Description,
                StartDate = createTripDto.StartDate,
                EndDate = createTripDto.EndDate,
                Image = imagePath,
                UserId = userId
            };

            try
            {
                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving trip: {ex.Message}");
                return StatusCode(500, "Error creating trip.");
            }

            return CreatedAtAction(
                nameof(GetTrip),
                new { id = trip.Id },
                new
                {
                    trip.Id,
                    trip.TripName,
                    trip.StartDate,
                    trip.EndDate,
                    trip.Description,
                    Image = GetAbsoluteUrl(trip.Image), // Convert to absolute URL
                    trip.UserId
                }
            );
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null) return NotFound();
            return Ok(trip);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var trips = _context.Trips.ToList()
                .Select(t => new
                {
                    t.Id,
                    t.TripName,
                    t.StartDate,
                    t.EndDate,
                    t.Description,
                    Image = GetAbsoluteUrl(t.Image),
                    t.UserId
                }).ToList();

            return Ok(trips);
        }

        [Authorize]
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateTrip(int id, [FromForm] TripDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // 1) Fetch the existing trip
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
                return NotFound("Trip not found.");

            // 2) Make sure the caller owns it
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (trip.UserId != userId)
                return Forbid("You are not authorized to edit this trip.");

            // 3) Handle image update
            string? newImagePath = trip.Image;
            if (dto.ImageFile != null)
            {
                // (Optional) delete old file if it was uploaded
                if (!string.IsNullOrEmpty(trip.Image) && trip.Image.StartsWith("/Uploads/"))
                {
                    var oldPath = Path.Combine(_environment.WebRootPath, trip.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                // Save the new file
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "Uploads");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
                var fullPath = Path.Combine(uploadsFolder, fileName);

                if (dto.ImageFile.Length > 5 * 1024 * 1024)
                    return BadRequest("File size must be less than 5MB");

                using (var stream = new FileStream(fullPath, FileMode.Create))
                    await dto.ImageFile.CopyToAsync(stream);

                newImagePath = $"/Uploads/{fileName}";
            }
            else if (!string.IsNullOrWhiteSpace(dto.ImageUrl))
            {
                newImagePath = dto.ImageUrl;
            }

            // 4) Update scalar properties
            trip.TripName = dto.TripName;
            trip.Description = dto.Description;
            trip.StartDate = dto.StartDate;
            trip.EndDate = dto.EndDate;
            trip.Image = newImagePath!;

            // 5) Save
            try
            {
                _context.Trips.Update(trip);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating trip {id}: {ex.Message}");
                return StatusCode(500, "Error updating trip.");
            }

            // 6) Return the updated resource
            return Ok(new
            {
                trip.Id,
                trip.TripName,
                trip.StartDate,
                trip.EndDate,
                trip.Description,
                Image = GetAbsoluteUrl(trip.Image),
                trip.UserId
            });
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            // Find the trip by ID
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound("Trip not found.");
            }

            // Check if the current user owns the trip
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (trip.UserId != userId)
            {
                return Forbid("You are not authorized to delete this trip.");
            }

            // Optionally delete the image file (if stored locally)
            if (!string.IsNullOrEmpty(trip.Image) && trip.Image.StartsWith("/Uploads/"))
            {
                var fullImagePath = Path.Combine(_environment.WebRootPath, trip.Image.TrimStart('/'));
                if (System.IO.File.Exists(fullImagePath))
                {
                    System.IO.File.Delete(fullImagePath);
                }
            }

            // Remove the trip
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }

        private string GetAbsoluteUrl(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return imagePath;

            // If already absolute, return as-is
            if (Uri.IsWellFormedUriString(imagePath, UriKind.Absolute))
                return imagePath;

            // Convert relative path to absolute URL
            return $"{Request.Scheme}://{Request.Host}{imagePath}";
        }
    }
}
