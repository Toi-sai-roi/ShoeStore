using System.ComponentModel.DataAnnotations;

namespace ShoeStore.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int VariantId { get; set; }
        public ProductVariant? Variant { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Computed properties dùng để hiển thị
        public string ProductName => Variant?.Product?.Name ?? "";
        public string Size => Variant?.Size ?? "";
        public string Color => Variant?.Color ?? "";
        public decimal UnitPrice => Variant?.Product?.Price ?? 0;
        public string? ImageUrl => Variant?.Product?.ImageUrl;
        public decimal SubTotal => UnitPrice * Quantity;
    }
}