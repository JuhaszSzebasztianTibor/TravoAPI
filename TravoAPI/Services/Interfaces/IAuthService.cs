using Microsoft.AspNetCore.Identity;
using TravoAPI.Dtos.Account;
using TravoAPI.Models;

namespace TravoAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto dto, IDictionary<string, List<string>> errors);
        Task<(bool Succeeded, ApplicationUser User, IDictionary<string, List<string>> Errors)>
            ValidateUserAsync(LoginDto dto);
        string GenerateJwtToken(ApplicationUser user);
    }
}
