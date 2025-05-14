using Microsoft.EntityFrameworkCore;
using TravoAPI.Models;
using TravoAPI.Models.Enums;

namespace TravoAPI.Seed
{
    public class PackingSeedData
    {
        public static void Seed(ModelBuilder builder)
        {
            // Seed packing lists with icon (without Category)
            builder.Entity<PackingList>().HasData(
                new PackingList { Id = 1, UserId = "SYSTEM", TripId = 0, Name = "Fancy Dinner", PackingListIcon = "fas fa-utensils" },
                new PackingList { Id = 2, UserId = "SYSTEM", TripId = 0, Name = "Beach", PackingListIcon = "fas fa-umbrella-beach" },
                new PackingList { Id = 3, UserId = "SYSTEM", TripId = 0, Name = "Business", PackingListIcon = "fas fa-briefcase" },
                new PackingList { Id = 4, UserId = "SYSTEM", TripId = 0, Name = "Essentials", PackingListIcon = "fas fa-exclamation-circle" }
            );

            // Seed packing items for those lists
            builder.Entity<PackingItem>().HasData(
                // Fancy Dinner items (listId = 1)
                new PackingItem { Id = 1, PackingListId = 1, Name = "Dress Shoes", Quantity = 1, IsChecked = false },
                new PackingItem { Id = 2, PackingListId = 1, Name = "Hair Dryer", Quantity = 1, IsChecked = false },
                new PackingItem { Id = 3, PackingListId = 1, Name = "Hair Products", Quantity = 1, IsChecked = false },
                new PackingItem { Id = 4, PackingListId = 1, Name = "Jacket", Quantity = 1, IsChecked = false },
                new PackingItem { Id = 5, PackingListId = 1, Name = "Jewelry", Quantity = 1, IsChecked = false },
                new PackingItem { Id = 6, PackingListId = 1, Name = "Pants", Quantity = 1, IsChecked = false },
                new PackingItem { Id = 7, PackingListId = 1, Name = "Pantyhose", Quantity = 1, IsChecked = false },
                new PackingItem { Id = 8, PackingListId = 1, Name = "Skirt", Quantity = 1, IsChecked = false },

                // Beach items (listId = 2)
                new PackingItem { Id = 50, PackingListId = 2, Name = "Sunscreen", Quantity = 1, IsChecked = false },
                new PackingItem { Id = 51, PackingListId = 2, Name = "Swimsuit", Quantity = 1, IsChecked = false },
                new PackingItem { Id = 52, PackingListId = 2, Name = "Towel", Quantity = 1, IsChecked = false },
                new PackingItem { Id = 53, PackingListId = 2, Name = "Flip Flops", Quantity = 1, IsChecked = false },
                new PackingItem { Id = 54, PackingListId = 2, Name = "Sunglasses", Quantity = 1, IsChecked = false },
                new PackingItem { Id = 55, PackingListId = 2, Name = "Beach Hat", Quantity = 1, IsChecked = false },
                new PackingItem { Id = 56, PackingListId = 2, Name = "Beach Ball", Quantity = 1, IsChecked = false },
                new PackingItem { Id = 57, PackingListId = 2, Name = "Cooler", Quantity = 1, IsChecked = false }
            );
        }
    }
}
