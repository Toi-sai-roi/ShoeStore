using System.ComponentModel.DataAnnotations;

namespace ShoeStore.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required]
        public string Size { get; set; } = "";   // "39", "40", "41"...

        [Required]
        public string Color { get; set; } = "";  // "Đen", "Trắng"...

        public int Stock { get; set; } = 0;
    }
}