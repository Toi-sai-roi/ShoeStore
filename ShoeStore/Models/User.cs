using System.ComponentModel.DataAnnotations;

namespace ShoeStore.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [RegularExpression(@"^[0-9]{10,11}$", ErrorMessage = "Số điện thoại phải 10-11 chữ số")]
        [StringLength(11, MinimumLength = 10)]
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public string Role { get; set; } = "Customer"; // Admin / Staff / Customer

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}