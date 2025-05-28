namespace TravoAPI.Dtos.Planner
{
    public class DayPlanDto
    {
        public int Id { get; set; }

        public int TripId { get; set; }
        public int DestinationId { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public List<PlaceDto> Places { get; set; }
    }
}
