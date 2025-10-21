using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftShack.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        public string? Description { get; set; }
    }
}