using System.ComponentModel.DataAnnotations;

namespace TravoAPI.Dtos.CreateTrip
{
    public class CreateTripDto
    {
        public string TripName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public IFormFile? ImageFile { get; set; }   // nullable
        public string? ImageUrl { get; set; }   // allow a URL

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            var hasFile = ImageFile != null;
            var hasUrl = !string.IsNullOrWhiteSpace(ImageUrl);

            if (hasFile && hasUrl)
            {
                yield return new ValidationResult(
                    "Cannot provide both ImageFile and ImageUrl",
                    new[] { nameof(ImageFile), nameof(ImageUrl) }
                );
            }

            if (!hasFile && !hasUrl)
            {
                yield return new ValidationResult(
                    "Must provide either ImageFile or ImageUrl",
                    new[] { nameof(ImageFile), nameof(ImageUrl) }
                );
            }
        }

    }
}
