using Microsoft.AspNetCore.Mvc;
using ShoeStore.Data;
using ShoeStore.Models;

namespace ShoeStore.Controllers
{
    public class VariantController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VariantController(ApplicationDbContext db) => _db = db;
        private bool IsAdminOrStaff() => new[] { "Admin", "Staff" }.Contains(HttpContext.Session.GetString("UserRole"));

        // GET: /Variant/Manage/5
        public IActionResult Manage(int productId)
        {
            if (!IsAdminOrStaff()) return RedirectToAction("Index", "Home");

            var product = _db.Products.Find(productId);
            if (product == null) return NotFound();

            var variants = _db.ProductVariants
                              .Where(v => v.ProductId == productId)
                              .ToList();

            ViewBag.ProductId = productId;
            ViewBag.ProductName = product.Name;
            return View(variants);
        }

        // GET: /Variant/Create?productId=5
        public IActionResult Create(int productId)
        {
            if (!IsAdminOrStaff()) return RedirectToAction("Index", "Home");

            ViewBag.ProductId = productId;
            return View();
        }

        // POST: /Variant/Create
        [HttpPost]
        public IActionResult Create(ProductVariant variant)
        {
            if (!IsAdminOrStaff()) return RedirectToAction("Index", "Home");

            _db.ProductVariants.Add(variant);
            _db.SaveChanges();
            return RedirectToAction("Manage", new { productId = variant.ProductId });
        }

        // GET: /Variant/Edit/5
        public IActionResult Edit(int id)
        {
            if (!IsAdminOrStaff()) return RedirectToAction("Index", "Home");

            var variant = _db.ProductVariants.Find(id);
            if (variant == null) return NotFound();
            return View(variant);
        }

        // POST: /Variant/Edit
        [HttpPost]
        public IActionResult Edit(ProductVariant variant)
        {
            if (!IsAdminOrStaff()) return RedirectToAction("Index", "Home");

            _db.ProductVariants.Update(variant);
            _db.SaveChanges();
            return RedirectToAction("Manage", new { productId = variant.ProductId });
        }

        // POST: /Variant/Delete
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!IsAdminOrStaff()) return RedirectToAction("Index", "Home");

            var variant = _db.ProductVariants.Find(id);
            if (variant == null) return NotFound();
            var productId = variant.ProductId;
            _db.ProductVariants.Remove(variant);
            _db.SaveChanges();
            return RedirectToAction("Manage", new { productId });
        }
    }
}