using Microsoft.EntityFrameworkCore;
using TravoAPI.Models;
using TravoAPI.Models.Enums;

namespace TravoAPI.Seed
{
    public class PackingSeedData
    {
        public static void Seed(ModelBuilder builder)
        {
            // Seed lists with required 'Name' field
            builder.Entity<PackingList>().HasData(new[]
            {
                new PackingList { Id = 1, Category = PackingCategory.FancyDinner, UserId = "SYSTEM", Name = "Fancy Dinner" },
                new PackingList { Id = 2, Category = PackingCategory.Beach,        UserId = "SYSTEM", Name = "Beach" },
                new PackingList { Id = 3, Category = PackingCategory.Business,     UserId = "SYSTEM", Name = "Business" },
                new PackingList { Id = 4, Category = PackingCategory.Baby,         UserId = "SYSTEM", Name = "Baby" },
                new PackingList { Id = 5, Category = PackingCategory.Essentials,   UserId = "SYSTEM", Name = "Essentials" },
                new PackingList { Id = 6, Category = PackingCategory.Food,         UserId = "SYSTEM", Name = "Food" }
            });


            // Seed items
            builder.Entity<PackingItem>().HasData(
                new PackingItem { Id = 1, PackingListId = 1, Name = "Dress Shoes", Quantity = 1 },
                new PackingItem { Id = 2, PackingListId = 1, Name = "Hair Dryer", Quantity = 1 },
                new PackingItem { Id = 3, PackingListId = 1, Name = "Hair Products", Quantity = 1 },
                new PackingItem { Id = 4, PackingListId = 1, Name = "Jacket", Quantity = 1 },
                new PackingItem { Id = 5, PackingListId = 1, Name = "Jewelry", Quantity = 1 },
                new PackingItem { Id = 6, PackingListId = 1, Name = "Pants", Quantity = 1 },
                new PackingItem { Id = 7, PackingListId = 1, Name = "Pantyhose", Quantity = 1 },
                new PackingItem { Id = 8, PackingListId = 1, Name = "Skirt", Quantity = 1 },

                new PackingItem { Id = 50, PackingListId = 2, Name = "Sunscreen", Quantity = 1 },
                new PackingItem { Id = 51, PackingListId = 2, Name = "Swimsuit", Quantity = 1 },
                new PackingItem { Id = 52, PackingListId = 2, Name = "Towel", Quantity = 1 },
                new PackingItem { Id = 53, PackingListId = 2, Name = "Flip Flops", Quantity = 1 },
                new PackingItem { Id = 54, PackingListId = 2, Name = "Sunglasses", Quantity = 1 },
                new PackingItem { Id = 55, PackingListId = 2, Name = "Beach Hat", Quantity = 1 },
                new PackingItem { Id = 56, PackingListId = 2, Name = "Beach Ball", Quantity = 1 },
                new PackingItem { Id = 57, PackingListId = 2, Name = "Cooler", Quantity = 1 }
            );
        }
    }
}
