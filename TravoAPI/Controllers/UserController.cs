using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravoAPI.Dtos.Account;
using TravoAPI.Models;

namespace TravoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public UserController(
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        /// <summary>
        /// Upload or replace the current user’s avatar.
        /// </summary>
        [Authorize]
        [HttpPost("me/photo")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadPhoto([FromForm] UploadPhotoDto dto)
        {
            // 1) get the current user’s ID from the JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            // 2) fetch the user entity
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            // 3) save the file to wwwroot/uploads/avatars
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "avatars");
            Directory.CreateDirectory(uploadsFolder);

            // use userId + extension for filename
            var ext = Path.GetExtension(dto.Photo.FileName);
            var fileName = $"{userId}{ext}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fs = new FileStream(filePath, FileMode.Create))
                await dto.Photo.CopyToAsync(fs);

            // 4) update the user’s PhotoUrl (publicly‐servable path)
            user.PhotoUrl = $"/uploads/avatars/{fileName}";
            await _userManager.UpdateAsync(user);

            // 5) return the new URL so the client can update its state
            return Ok(new { photoUrl = user.PhotoUrl });
        }
    }
}
