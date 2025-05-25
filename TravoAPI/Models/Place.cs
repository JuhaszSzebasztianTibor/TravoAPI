// Models/Place.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravoAPI.Models
{
    public class Place
    {
        public int Id { get; set; }
        public int DayPlanId { get; set; }
        public DayPlan DayPlan { get; set; }

        [Required]
        public string Location { get; set; }

        public string Type { get; set; }
        public string Time { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string PhotoUrl { get; set; }

        public ICollection<Note> Notes { get; set; }
    }
}
