using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EntityFrameworkCore.Models
{
    public class CarServiceContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarTechState> CarTechStates { get; set; }
        public DbSet<Inspector> Inspectors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            string connectionString = config.GetConnectionString("SQLConnection");

            var options = optionsBuilder.UseSqlServer(connectionString).Options;
        }
    }
}
