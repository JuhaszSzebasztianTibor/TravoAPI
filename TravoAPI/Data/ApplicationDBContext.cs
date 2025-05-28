// Data/ApplicationDBContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravoAPI.Models;
using TravoAPI.Seed;

namespace TravoAPI.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<PackingList> PackingLists { get; set; }
        public DbSet<PackingItem> PackingItems { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<DayPlan> DayPlans { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // 1) KEEP identity's defaults
            base.OnModelCreating(builder);

            builder.Entity<Destination>()
                     .HasMany(d => d.DayPlans)
                     .WithOne(dp => dp.Destination)
                     .HasForeignKey(dp => dp.DestinationId)
                     .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DayPlan>()
                   .HasMany(dp => dp.Places)
                   .WithOne(p => p.DayPlan)
                   .HasForeignKey(p => p.DayPlanId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 4) YOUR SEED DATA (unchanged)
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
                SecurityStamp = "00000000-0000-0000-0000-000000000000",
                ConcurrencyStamp = "11111111-1111-1111-1111-111111111111",
                PasswordHash = "AQAAAAEAACcQAAAAEKX8d2J2lULBw4mYx4Zx05wZIjgj6UeQ7GFXSJiJTh+ZJ6Rqiw1j4fYSUQ2mLzdCjg=="
            });
            PackingSeedData.Seed(builder);
        }
    }
}
