using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TravoAPI.Dtos.Planner;

namespace TravoAPI.Dtos.CreateTrip
{
    public class TripDto
    {
        public int Id { get; set; }
        public string TripName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public IFormFile? ImageFile { get; set; } 
        public string? ImageUrl { get; set; }

        public List<DestinationDto>? Destinations { get; set; }
    }
}
