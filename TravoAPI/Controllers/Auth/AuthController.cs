using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using TravoAPI.Dtos.Account;
using TravoAPI.Services;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var errors = new Dictionary<string, List<string>>();
            var result = await _auth.RegisterAsync(dto, errors);
            if (!result.Succeeded)
                return BadRequest(new { errors });

            return Ok(new { message = "Registration successful" });
        }

        /// <summary>
        /// Validates credentials and returns a JWT plus basic user info.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var (ok, user, errors) = await _auth.ValidateUserAsync(dto);
            if (!ok)
                return BadRequest(new { errors });

            var token = _auth.GenerateJwtToken(user);
            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    PhotoUrl = user.PhotoUrl
                }
            });
        }

        /// <summary>
        /// Returns the currently authenticated user’s basic profile.
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var claims = User.Claims;
            return Ok(new
            {
                Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Email = User.FindFirstValue(JwtRegisteredClaimNames.Email),
                FirstName = User.FindFirstValue("firstName"),
                LastName = User.FindFirstValue("lastName"),
                PhotoUrl = User.FindFirstValue("photoUrl") 
            });
        }
    }
}
