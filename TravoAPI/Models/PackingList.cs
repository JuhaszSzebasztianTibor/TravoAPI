using TravoAPI.Models.Enums;

namespace TravoAPI.Models
{
    public class PackingList
    {
        public int Id { get; set; }

        public PackingCategory Category { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // Constructor initializing the collection
        public List<PackingItem> Items { get; set; }

        public string Name { get; set; }

        public PackingList()
        {
            Items = new List<PackingItem>();  // Ensure the collection is initialized in the constructor
        }
    }
}
