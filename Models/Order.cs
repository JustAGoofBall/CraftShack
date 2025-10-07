namespace CraftShack.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? CustomerName { get; set; }
        public string? TableNumber { get; set; } // For local orders
        public string? Address { get; set; }     // For online orders
        public string? OrderType { get; set; }    // "Local" or "Online"
        public string? Status { get; set; }       // "Pending", "Completed", etc.

        // Navigation property
        public Product? Product { get; set; }
    }
}