using System.ComponentModel.DataAnnotations;

namespace ShoeStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}