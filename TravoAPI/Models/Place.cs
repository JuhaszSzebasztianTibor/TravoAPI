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
        public string location { get; set; }
        public string Type { get; set; }
        public string Time { get; set; }
        public ICollection<Note> Notes { get; set; }
    }
}
