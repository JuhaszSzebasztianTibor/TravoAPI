namespace TravoAPI.Models
{
    public class PackingItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public bool IsChecked { get; set; }

        public int PackingListId { get; set; }
        public PackingList PackingList { get; set; }
    }
}
