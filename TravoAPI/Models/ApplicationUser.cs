using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TravoAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(256)]
        public string? PhotoUrl { get; set; }
    }
}
