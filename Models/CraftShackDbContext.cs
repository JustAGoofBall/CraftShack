using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Add this
using CraftShack.Data;

namespace CraftShack.Models
{
    public class CraftShackDbContext : IdentityDbContext 
    {
        public CraftShackDbContext(DbContextOptions<CraftShackDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ProductSeed.Seed(modelBuilder);
        }
    }
}