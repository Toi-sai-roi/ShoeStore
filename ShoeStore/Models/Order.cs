using System.ComponentModel.DataAnnotations;

namespace ShoeStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public string CustomerName { get; set; } = "";

        public string? CustomerPhone { get; set; }

        public string? CustomerAddress { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string PaymentMethod { get; set; } = "COD";

        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Delivered, Cancelled

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}