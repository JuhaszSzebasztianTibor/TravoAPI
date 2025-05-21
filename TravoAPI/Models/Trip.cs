namespace TravoAPI.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string TripName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Image { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<Destination> Destinations { get; set; }
        public ICollection<DayPlan> DayPlans { get; set; }
    }
}
