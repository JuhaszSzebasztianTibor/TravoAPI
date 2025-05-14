using TravoAPI.Models.Enums;

namespace TravoAPI.Models
{
    public class PackingList
    {
        public int Id { get; set; }

        public int TripId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string Name { get; set; }
        public string PackingListIcon { get; set; } = "fas fa-clipboard-list";

        public List<PackingItem> Items { get; set; } = new List<PackingItem>();
    }
}
