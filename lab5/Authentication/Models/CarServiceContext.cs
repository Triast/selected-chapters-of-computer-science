using Microsoft.EntityFrameworkCore;

namespace Authentication.Models
{
    public class CarServiceContext : DbContext
    {
        public CarServiceContext(DbContextOptions<CarServiceContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<CarTechState> CarTechStates { get; set; }
        public DbSet<Inspector> Inspectors { get; set; }
    }
}
