using ShoeStore.Models;

namespace ShoeStore.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext db)
        {
            // Seed users
            if (!db.Users.Any())
            {
                db.Users.AddRange(
                    new User
                    {
                        FullName = "Lê Văn Tùng",
                        Email = "tung@shop.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("123"),
                        Phone = "0398176120",
                        Address = "Hà Nội",
                        Role = "Customer"
                    },
                    new User
                    {
                        FullName = "Nguyễn Thanh Bình",
                        Email = "binh@shop.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("123"),
                        Phone = "0934689005",
                        Address = "Hồ Chí Minh",
                        Role = "Staff"
                    },
                    new User
                    {
                        FullName = "Admin",
                        Email = "admin@shop.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                        Phone = "0999999999",
                        Address = "Hà Nội",
                        Role = "Admin"
                    }
                );
                db.SaveChanges();
            }

            // Nếu đã có data rồi thì bỏ qua
            if (db.Categories.Any()) return;

            var categories = new List<Category>
            {
                new Category { Name = "Giày Thể Thao" },
                new Category { Name = "Dép" },
                new Category { Name = "Giày Cổ Cao" },
                new Category { Name = "Giày Lười" },
                new Category { Name = "Giày Cao Gót" },
                new Category { Name = "Giày Canvas" },
                new Category { Name = "Giày Trekking" }
            };
            db.Categories.AddRange(categories);
            db.SaveChanges();

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Adidas Adizero Adios Pro 3",
                    Description = "Giày chạy bộ chuyên nghiệp với công nghệ Boost.",
                    Price = 3500000,
                    ImageUrl = "Adidas Adizero Adios Pro 3.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "39", Color = "Xanh Đen", Stock = 8 },
                        new ProductVariant { Size = "40", Color = "Xanh Đen", Stock = 6 },
                        new ProductVariant { Size = "41", Color = "Xanh Đen", Stock = 5 }
                    }
                },
                new Product
                {
                    Name = "Adidas Ultraboost 22",
                    Description = "Đệm Boost êm ái, lý tưởng cho chạy bộ.",
                    Price = 3200000,
                    ImageUrl = "Adidas Ultraboost 22.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Xanh", Stock = 6 },
                        new ProductVariant { Size = "41", Color = "Xanh", Stock = 4 },
                        new ProductVariant { Size = "42", Color = "Trắng", Stock = 7 }
                    }
                },
                new Product
                {
                    Name = "Adidas Ultraboost Light",
                    Description = "Giày ultralight với công nghệ Boost mới.",
                    Price = 2800000,
                    ImageUrl = "Adidas Ultraboost Light.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "39", Color = "Trắng Đỏ", Stock = 7 },
                        new ProductVariant { Size = "40", Color = "Trắng Đỏ", Stock = 5 },
                        new ProductVariant { Size = "41", Color = "Trắng Đỏ", Stock = 6 }
                    }
                },
                new Product
                {
                    Name = "Asics Gel-Kayano 31",
                    Description = "Giày chạy bộ hỗ trợ chân cao cấp.",
                    Price = 3800000,
                    ImageUrl = "Asics Gel-Kayano 31.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Xanh Nhạt", Stock = 5 },
                        new ProductVariant { Size = "41", Color = "Xanh Nhạt", Stock = 4 },
                        new ProductVariant { Size = "42", Color = "Xanh Nhạt", Stock = 3 }
                    }
                },
                new Product
                {
                    Name = "Birkenstock Arizona",
                    Description = "Dép da bò thoải mái cho mùa hè.",
                    Price = 1800000,
                    ImageUrl = "Birkenstock Arizona.png",
                    CategoryId = categories[1].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "38", Color = "Nâu", Stock = 9 },
                        new ProductVariant { Size = "39", Color = "Nâu", Stock = 5 },
                        new ProductVariant { Size = "40", Color = "Đen", Stock = 4 }
                    }
                },
                new Product
                {
                    Name = "Brooks Ghost 16",
                    Description = "Giày chạy bộ thoải mái hàng ngày.",
                    Price = 2900000,
                    ImageUrl = "Brooks Ghost 16.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Xám Trắng", Stock = 7 },
                        new ProductVariant { Size = "41", Color = "Xám Trắng", Stock = 6 },
                        new ProductVariant { Size = "42", Color = "Xám Trắng", Stock = 5 }
                    }
                },
                new Product
                {
                    Name = "Converse Chuck Taylor All Star",
                    Description = "Giày vải cổ điển, phong cách thể thao.",
                    Price = 1500000,
                    ImageUrl = "Converse Chuck Taylor All Star.png",
                    CategoryId = categories[6].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "39", Color = "Đen", Stock = 10 },
                        new ProductVariant { Size = "40", Color = "Đen", Stock = 8 },
                        new ProductVariant { Size = "41", Color = "Trắng", Stock = 7 }
                    }
                },
                new Product
                {
                    Name = "Dr. Martens 1460",
                    Description = "Boot da cổ cao, phong cách punk rock.",
                    Price = 4500000,
                    ImageUrl = "Dr. Martens 1460.png",
                    CategoryId = categories[2].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Đen", Stock = 3 },
                        new ProductVariant { Size = "41", Color = "Đen", Stock = 2 },
                        new ProductVariant { Size = "42", Color = "Nâu", Stock = 4 }
                    }
                },
                new Product
                {
                    Name = "Giày Da Nam Oxford",
                    Description = "Giày da lịch sự cho những dịp quan trọng.",
                    Price = 2200000,
                    ImageUrl = "Giày Da Nam Oxford.png",
                    CategoryId = categories[3].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "39", Color = "Nâu", Stock = 5 },
                        new ProductVariant { Size = "40", Color = "Nâu", Stock = 4 },
                        new ProductVariant { Size = "41", Color = "Đen", Stock = 6 }
                    }
                },
                new Product
                {
                    Name = "Hoka Clifton Captoe",
                    Description = "Giày chạy bộ nhẹ và êm ái.",
                    Price = 3100000,
                    ImageUrl = "Hoka Clifton Captoe.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Xanh", Stock = 6 },
                        new ProductVariant { Size = "41", Color = "Xanh", Stock = 5 },
                        new ProductVariant { Size = "42", Color = "Xanh", Stock = 4 }
                    }
                },
                new Product
                {
                    Name = "Juno High Heels",
                    Description = "Giày cao gót nữ thời trang.",
                    Price = 1900000,
                    ImageUrl = "Juno High Heels 7cm.png",
                    CategoryId = categories[4].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "36", Color = "Kem", Stock = 4 },
                        new ProductVariant { Size = "37", Color = "Kem", Stock = 3 },
                        new ProductVariant { Size = "38", Color = "Kem", Stock = 2 }
                    }
                },
                new Product
                {
                    Name = "Mizuno Wave Rider 28",
                    Description = "Giày chạy bộ chuyên nghiệp từ Nhật.",
                    Price = 3400000,
                    ImageUrl = "Mizuno Wave Rider 28.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Xanh Tím", Stock = 5 },
                        new ProductVariant { Size = "41", Color = "Xanh Tím", Stock = 4 },
                        new ProductVariant { Size = "42", Color = "Xanh Tím", Stock = 3 }
                    }
                },
                new Product
                {
                    Name = "New Balance Fresh Foam 1080v13",
                    Description = "Giày chạy bộ với foam êm ái.",
                    Price = 3600000,
                    ImageUrl = "New Balance Fresh Foam 1080v13.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Trắng Đỏ", Stock = 7 },
                        new ProductVariant { Size = "41", Color = "Trắng Đỏ", Stock = 5 },
                        new ProductVariant { Size = "42", Color = "Trắng Đỏ", Stock = 4 }
                    }
                },
                new Product
                {
                    Name = "Nike Air Force 1",
                    Description = "Giày sneaker cổ điển, phong cách đường phố.",
                    Price = 2500000,
                    ImageUrl = "Nike Air Force 1.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "39", Color = "Trắng", Stock = 10 },
                        new ProductVariant { Size = "40", Color = "Trắng", Stock = 8 },
                        new ProductVariant { Size = "41", Color = "Đen", Stock = 5 }
                    }
                },
                new Product
                {
                    Name = "Nike Air Max 270 React",
                    Description = "Giày thể thao với công nghệ Air Max.",
                    Price = 3300000,
                    ImageUrl = "Nike Air Max 270 React.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Đen Xanh Cam", Stock = 6 },
                        new ProductVariant { Size = "41", Color = "Đen Xanh Cam", Stock = 5 },
                        new ProductVariant { Size = "42", Color = "Đen Xanh Cam", Stock = 4 }
                    }
                },
                new Product
                {
                    Name = "Nike Invincible Run 3",
                    Description = "Giày chạy bộ siêu êm với công nghệ React.",
                    Price = 2700000,
                    ImageUrl = "Nike Invincible Run 3.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Xanh Sáng", Stock = 5 },
                        new ProductVariant { Size = "41", Color = "Xanh Sáng", Stock = 4 },
                        new ProductVariant { Size = "42", Color = "Xanh Sáng", Stock = 3 }
                    }
                },
                new Product
                {
                    Name = "On Cloudmonster",
                    Description = "Giày chạy bộ với công nghệ Helion.",
                    Price = 3000000,
                    ImageUrl = "On Cloudmonster.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Xám Đen", Stock = 6 },
                        new ProductVariant { Size = "41", Color = "Xám Đen", Stock = 5 },
                        new ProductVariant { Size = "42", Color = "Xám Đen", Stock = 4 }
                    }
                },
                new Product
                {
                    Name = "Puma Deviate Nitro 2",
                    Description = "Giày chạy bộ tốc độ cao từ Puma.",
                    Price = 2600000,
                    ImageUrl = "Puma Deviate Nitro 2.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Cam Đen", Stock = 7 },
                        new ProductVariant { Size = "41", Color = "Cam Đen", Stock = 6 },
                        new ProductVariant { Size = "42", Color = "Cam Đen", Stock = 5 }
                    }
                },
                new Product
                {
                    Name = "Puma Suede Classic",
                    Description = "Giày sneaker da lộn cổ điển.",
                    Price = 1700000,
                    ImageUrl = "Puma Suede Classic.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "39", Color = "Xanh", Stock = 8 },
                        new ProductVariant { Size = "40", Color = "Xanh", Stock = 7 },
                        new ProductVariant { Size = "41", Color = "Xanh", Stock = 6 }
                    }
                },
                new Product
                {
                    Name = "Salomon X Ultra 5 GTX",
                    Description = "Giày leo núi chuyên nghiệp chống nước.",
                    Price = 4200000,
                    ImageUrl = "Salomon X Ultra 5 GTX.png",
                    CategoryId = categories[6].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Đen Cam", Stock = 4 },
                        new ProductVariant { Size = "41", Color = "Đen Cam", Stock = 3 },
                        new ProductVariant { Size = "42", Color = "Đen Cam", Stock = 2 }
                    }
                },
                new Product
                {
                    Name = "Saucony Endorphin Speed 4",
                    Description = "Giày chạy bộ đua nhanh và nhẹ.",
                    Price = 2950000,
                    ImageUrl = "Saucony Endorphin Speed 4.png",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Trắng Cam", Stock = 5 },
                        new ProductVariant { Size = "41", Color = "Trắng Cam", Stock = 4 },
                        new ProductVariant { Size = "42", Color = "Trắng Cam", Stock = 3 }
                    }
                },
                new Product
                {
                    Name = "Vascara Ballerina Flats",
                    Description = "Giày búp bê nữ dễ thương.",
                    Price = 980000,
                    ImageUrl = "Vascara Ballerina Flats.png",
                    CategoryId = categories[4].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "36", Color = "Kem", Stock = 10 },
                        new ProductVariant { Size = "37", Color = "Kem", Stock = 8 },
                        new ProductVariant { Size = "38", Color = "Kem", Stock = 6 }
                    }
                }
            };
            db.Products.AddRange(products);
            db.SaveChanges();
        }
    }
}