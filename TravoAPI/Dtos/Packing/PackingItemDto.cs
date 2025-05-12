namespace TravoAPI.Dtos.Packing
{
    public class PackingItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsChecked { get; set; }
        public int? Quantity { get; set; }
    }
}