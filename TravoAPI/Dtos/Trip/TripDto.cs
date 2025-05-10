using System.ComponentModel.DataAnnotations;

namespace TravoAPI.Dtos.CreateTrip
{
    public class TripDto
    {
        public string TripName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public IFormFile? ImageFile { get; set; }   // nullable
        public string? ImageUrl { get; set; }   // allow a URL

    }
}
