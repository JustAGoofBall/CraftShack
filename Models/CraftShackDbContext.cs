using Microsoft.EntityFrameworkCore;
using CraftShack.Data;

namespace CraftShack.Models
{
    public class CraftShackDbContext : DbContext
    {
        public CraftShackDbContext(DbContextOptions<CraftShackDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ProductSeed.Seed(modelBuilder);
            UserSeed.Seed(modelBuilder);
        }
    }
}