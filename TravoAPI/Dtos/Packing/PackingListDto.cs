﻿namespace TravoAPI.Dtos.Packing
{
    public class PackingListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PackingListIcon { get; set; }
        public List<PackingItemDto> Items { get; set; } = new List<PackingItemDto>();
    }
}
