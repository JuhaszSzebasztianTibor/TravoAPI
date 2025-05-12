using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using TravoAPI.Dtos.Account;
using TravoAPI.Models;
using TravoAPI.Services.Interfaces;

namespace TravoAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto, IDictionary<string, List<string>> errors)
        {
            // Email uniqueness
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                errors["email"] = new List<string> { "Email already in use." };

            // Password complexity
            if (dto.Password.Length < 8 ||
                !Regex.IsMatch(dto.Password, "[A-Z]") ||
                !Regex.IsMatch(dto.Password, "[a-z]") ||
                !Regex.IsMatch(dto.Password, "\\d"))
            {
                errors["password"] = new List<string>
                {
                    "Password must be ≥8 chars, include upper, lower, and number."
                };
            }

            if (errors.Count > 0)
                return IdentityResult.Failed();

            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            return await _userManager.CreateAsync(user, dto.Password);
        }

        public async Task<(bool, ApplicationUser, IDictionary<string, List<string>>)>
            ValidateUserAsync(LoginDto dto)
        {
            var errors = new Dictionary<string, List<string>>();

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                errors["email"] = new List<string> { "No account found." };
                return (false, null, errors);
            }

            var signIn = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!signIn.Succeeded)
            {
                errors["password"] = new List<string> { "Password incorrect." };
                return (false, user, errors);
            }

            return (true, user, errors);
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
