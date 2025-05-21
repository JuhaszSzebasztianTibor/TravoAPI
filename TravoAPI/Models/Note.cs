// Models/Note.cs
using System.ComponentModel.DataAnnotations;

namespace TravoAPI.Models
{
    public class Note
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public Place Place { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
