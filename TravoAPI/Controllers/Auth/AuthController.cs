using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TravoAPI.Dtos.Account;
using TravoAPI.Models;

namespace YourNamespace.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var errors = new Dictionary<string, List<string>>();

            // 1) EMAIL
            if (string.IsNullOrWhiteSpace(dto.Email))
                AddError(errors, "email", "Email is required");
            else if (!IsValidEmail(dto.Email))
                AddError(errors, "email", "Invalid email format");
            else if (await _userManager.FindByEmailAsync(dto.Email) != null)
                AddError(errors, "email", "This email is already registered");

            // 2) PASSWORD
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                AddError(errors, "password", "Password is required");
            }
            else
            {
                if (dto.Password.Length < 8)
                    AddError(errors, "password", "Password must be at least 8 characters");
                if (!Regex.IsMatch(dto.Password, @"[A-Z]"))
                    AddError(errors, "password", "Password must contain at least one uppercase letter");
                if (!Regex.IsMatch(dto.Password, @"[a-z]"))
                    AddError(errors, "password", "Password must contain at least one lowercase letter");
                if (!Regex.IsMatch(dto.Password, @"\d"))
                    AddError(errors, "password", "Password must contain at least one number");
            }

            // 3) ONE SINGLE RETURN FOR ALL ERRORS
            if (errors.Any())
                return BadRequest(new { errors });

            // 4) create the user…
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    AddError(errors, "general", e.Description);
                return BadRequest(new { errors });
            }

            return Ok(new { message = "Registration successful" });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var errors = new Dictionary<string, List<string>>();

            // 1) Required‐field validation
            if (string.IsNullOrWhiteSpace(dto.Email))
                AddError(errors, "email", "Email is required");
            if (string.IsNullOrWhiteSpace(dto.Password))
                AddError(errors, "password", "Password is required");
            if (errors.Any())
                return BadRequest(new { errors });

            // 2) User existence
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                AddError(errors, "email", "No account found with that email");
                return BadRequest(new { errors });
            }

            // 3) Password check
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
            {
                AddError(errors, "password", "Password is incorrect");
                return BadRequest(new { errors });
            }

            // 4) Success → issue JWT
            var token = GenerateJwtToken(user);
            return Ok(new
            {
                token,
                user = new { user.Id, user.Email, user.FirstName, user.LastName }
            });
        }
        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true; 
            }
            catch
            {
                return false;
            }
        }

        private void AddError(Dictionary<string, List<string>> errors, string key, string message)
        {
            key = key.ToLower();
            if (!errors.ContainsKey(key)) errors[key] = new List<string>();
            if (!errors[key].Contains(message)) errors[key].Add(message);
        }
    }
}
