using System.ComponentModel.DataAnnotations;

namespace TravoAPI.Dtos.Account
{
    public class UploadPhotoDto
    {
        [Required]
        public IFormFile Photo { get; set; } = default!;
    }
}
