using System.ComponentModel.DataAnnotations;

namespace ShoeStore.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required]
        public string Size { get; set; } = "";  

        [Required]
        public string Color { get; set; } = ""; 

        public int Stock { get; set; } = 0;
    }
}