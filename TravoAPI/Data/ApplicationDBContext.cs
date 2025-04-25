using Microsoft.EntityFrameworkCore;

namespace TravoAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }

        //public DbSet<> MyProperty { get; set; }
    }
}
