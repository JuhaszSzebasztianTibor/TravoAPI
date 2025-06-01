// Models/Destination.cs
using System.ComponentModel.DataAnnotations;

namespace TravoAPI.Models
{
    public class Destination
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }

        [Required]
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        [Range(1, 365)]
        public int Nights { get; set; }

        public ICollection<DayPlan> DayPlans { get; set; }
    }
}
