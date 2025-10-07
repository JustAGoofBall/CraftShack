using Microsoft.EntityFrameworkCore;
using CraftShack.Models;

namespace CraftShack.Data
{
    public static class UserSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = "admin123", // Use a hash in production!
                    FullName = "Admin User",
                    Address = "Admin Address"
                }
            );
        }
    }
}