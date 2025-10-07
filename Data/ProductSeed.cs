using Microsoft.EntityFrameworkCore;
using CraftShack.Models;

namespace CraftShack.Data
{
    public static class ProductSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Creeper CupCake", Price = 19.99m, Description = "A delicious cupcake inspired by Minecraft's Creeper." },
                new Product { Id = 2, Name = "Zombie Steak", Price = 7.99m, Description = "Tasty steak with a Minecraft zombie twist." },
                new Product { Id = 3, Name = "Golden Carrot Candy", Price = 24.99m, Description = "Sweet golden carrot candy for Minecraft fans." },
                new Product { Id = 4, Name = "Diamond Donut", Price = 4.99m, Description = "A shiny donut that looks like a Minecraft diamond." }
            );
        }
    }
}
