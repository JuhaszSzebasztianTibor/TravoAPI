using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TravoAPI.Models;
using TravoAPI.Seed;
using Microsoft.AspNetCore.Identity;

namespace TravoAPI.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<PackingList> PackingLists { get; set; }
        public DbSet<PackingItem> PackingItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "SYSTEM",
                UserName = "system@localhost",
                NormalizedUserName = "SYSTEM@LOCALHOST",
                Email = "system@localhost",
                NormalizedEmail = "SYSTEM@LOCALHOST",
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Account",
                SecurityStamp = "00000000-0000-0000-0000-000000000000", // Static
                ConcurrencyStamp = "11111111-1111-1111-1111-111111111111", // Static
                PasswordHash = "AQAAAAEAACcQAAAAEKX8d2J2lULBw4mYx4Zx05wZIjgj6UeQ7GFXSJiJTh+ZJ6Rqiw1j4fYSUQ2mLzdCjg==" // Static
            });

            PackingSeedData.Seed(builder);
        }

    }
}
