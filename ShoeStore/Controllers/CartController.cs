using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Data;
using ShoeStore.Models;
using System.Text.Json;

namespace ShoeStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private const string CartKey = "Cart";

        public CartController(ApplicationDbContext db) => _db = db;

        // GET: /Cart
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var cart = _db.CartItems
                          .Where(c => c.UserId == userId)
                          .Include(c => c.Variant)
                          .ThenInclude(v => v!.Product)
                          .ToList();
            return View(cart);
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int variantId, int quantity = 1)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var variant = _db.ProductVariants
                             .Include(v => v.Product)
                             .FirstOrDefault(v => v.Id == variantId);
            if (variant == null) return NotFound();

            var existing = _db.CartItems
                              .FirstOrDefault(c => c.UserId == userId && c.VariantId == variantId);

            int currentInCart = existing?.Quantity ?? 0;
            int totalWanted = currentInCart + quantity;

            if (totalWanted > variant.Stock)
            {
                TempData["CartError"] = $"Chỉ còn {variant.Stock} sản phẩm trong kho, bạn đã có {currentInCart} trong giỏ.";
                return RedirectToAction("Index", "Home");
            }

            if (existing != null)
                existing.Quantity += quantity;
            else
                _db.CartItems.Add(new CartItem
                {
                    UserId = userId.Value,
                    VariantId = variantId,
                    Quantity = quantity,
                });

            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: /Cart/Remove
        [HttpPost]
        public IActionResult Remove(int variantId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var item = _db.CartItems
                          .FirstOrDefault(c => c.UserId == userId && c.VariantId == variantId);
            if (item != null) _db.CartItems.Remove(item);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: /Cart/Clear
        [HttpPost]
        public IActionResult Clear()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var items = _db.CartItems.Where(c => c.UserId == userId).ToList();
            _db.CartItems.RemoveRange(items);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}