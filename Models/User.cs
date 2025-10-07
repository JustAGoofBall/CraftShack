namespace CraftShack.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        // Add more as needed
    }
}