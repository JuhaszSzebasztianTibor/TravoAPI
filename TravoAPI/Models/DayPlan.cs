// Models/DayPlan.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravoAPI.Models
{
    public class DayPlan
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }

        public int DestinationId { get; set; }
        public Destination Destination { get; set; }
        public string Location { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public ICollection<Place> Places { get; set; }
    }
}
