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
            if (db.Products.Any()) return;

            var categories = new List<Category>
            {
                new Category { Name = "Sneaker" },
                new Category { Name = "Sandal" },
                new Category { Name = "Boot" }
            };
            db.Categories.AddRange(categories);
            db.SaveChanges();

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Nike Air Force 1",
                    Description = "Giày sneaker cổ điển, phong cách đường phố.",
                    Price = 2500000,
                    ImageUrl = "nike-af1.jpg",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "39", Color = "Trắng", Stock = 10 },
                        new ProductVariant { Size = "40", Color = "Trắng", Stock = 8 },
                        new ProductVariant { Size = "41", Color = "Đen",   Stock = 5 },
                        new ProductVariant { Size = "42", Color = "Đen",   Stock = 3 }
                    }
                },
                new Product
                {
                    Name = "Adidas Ultraboost 22",
                    Description = "Đệm Boost êm ái, lý tưởng cho chạy bộ.",
                    Price = 3200000,
                    ImageUrl = "adidas-ub22.jpg",
                    CategoryId = categories[0].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Xanh",  Stock = 6 },
                        new ProductVariant { Size = "41", Color = "Xanh",  Stock = 4 },
                        new ProductVariant { Size = "42", Color = "Trắng", Stock = 7 }
                    }
                },
                new Product
                {
                    Name = "Birkenstock Arizona",
                    Description = "Sandal da bò thoải mái cho mùa hè.",
                    Price = 1800000,
                    ImageUrl = "birkenstock.jpg",
                    CategoryId = categories[1].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "38", Color = "Nâu",  Stock = 9 },
                        new ProductVariant { Size = "39", Color = "Nâu",  Stock = 5 },
                        new ProductVariant { Size = "40", Color = "Đen",  Stock = 4 }
                    }
                },
                new Product
                {
                    Name = "Dr. Martens 1460",
                    Description = "Boot da cổ cao, phong cách punk rock.",
                    Price = 4500000,
                    ImageUrl = "drmartens.jpg",
                    CategoryId = categories[2].Id,
                    Variants = new List<ProductVariant>
                    {
                        new ProductVariant { Size = "40", Color = "Đen", Stock = 3 },
                        new ProductVariant { Size = "41", Color = "Đen", Stock = 2 },
                        new ProductVariant { Size = "42", Color = "Nâu", Stock = 4 }
                    }
                }
            };
            db.Products.AddRange(products);
            db.SaveChanges();
        }
    }
}