
namespace TravoAPI.Dtos.Planner
{
    public class PlaceDto
    {
        public int Id { get; set; }
        public int DayPlanId { get; set; }

        public int TripId { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Time { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string PhotoUrl { get; set; }

        public List<NoteDto> Notes { get; set; }
    }
}
