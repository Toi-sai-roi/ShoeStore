using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Data;
using ShoeStore.Models;

namespace ShoeStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db) => _db = db;

        public IActionResult Index(string? search, int? categoryId, string? sort, int page = 1)
        {
            // Restore session từ cookie nếu cần
            if (HttpContext.Session.GetInt32("UserId") == null && User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirst("UserId")?.Value;
                var email = User.FindFirst("UserEmail")?.Value;
                var role = User.FindFirst("UserRole")?.Value;

                if (userId != null && email != null && role != null)
                {
                    HttpContext.Session.SetInt32("UserId", int.Parse(userId));
                    HttpContext.Session.SetString("UserEmail", email);
                    HttpContext.Session.SetString("UserRole", role);
                }
            }

            int pageSize = 8;

            var query = _db.Products
                           .Include(p => p.Category)
                           .Include(p => p.Variants)
                           .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId);

            query = sort switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "newest" => query.OrderByDescending(p => p.Id),
                _ => query.OrderBy(p => p.Name)
            };

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var products = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Search = search;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.CategoryId = categoryId;
            ViewBag.Sort = sort;
            ViewBag.Categories = _db.Categories.ToList();

            return View(products);
        }

        public IActionResult SearchSuggestions(string? q)
        {
            if (string.IsNullOrWhiteSpace(q)) return Json(new List<object>());

            var results = _db.Products
                .Where(p => p.Name.Contains(q))
                .Take(20)
                .Select(p => new { p.Id, p.Name, p.Price, p.ImageUrl })
                .ToList();

            return Json(results);
        }

        public IActionResult Details(int id)
        {
            var product = _db.Products
                             .Include(p => p.Category)
                             .Include(p => p.Variants)
                             .Include(p => p.Reviews)
                                 .ThenInclude(r => r.User)
                             .FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public IActionResult PostReview(int productId, int rating, string? comment)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            _db.Reviews.Add(new Review
            {
                UserId = userId.Value,
                ProductId = productId,
                Rating = rating,
                Comment = comment
            });
            _db.SaveChanges();
            return RedirectToAction("Details", new { id = productId });
        }
    }
}