using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using SuperCarGarage.Models;

namespace SuperCarBookingSystem.Services
{
    public class CarDbContext : DbContext
    {
        public DbSet<Car> Cars { get; init; }      

        public CarDbContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>().ToCollection("cars");          }
    }
}
