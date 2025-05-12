using TravoAPI.Models.Enums;

namespace TravoAPI.Dtos.Packing
{
    public class PackingListDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public PackingCategory Category { get; set; }
        public List<PackingItemDto> Items { get; set; } = new();
    }
}
