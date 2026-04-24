using Microsoft.AspNetCore.Mvc;
using ShoeStore.Data;
using ShoeStore.Models;

namespace ShoeStore.Controllers
{
    [AdminOrStaff]
    public class VariantController : BaseController
    {
        private readonly ApplicationDbContext _db;
        public VariantController(ApplicationDbContext db) => _db = db;

        public IActionResult Manage(int productId)
        {
            var product = _db.Products.Find(productId);
            if (product == null) return NotFound();

            var variants = _db.ProductVariants
                              .Where(v => v.ProductId == productId)
                              .ToList();

            ViewBag.ProductId = productId;
            ViewBag.ProductName = product.Name;
            return View(variants);
        }

        public IActionResult Create(int productId)
        {
            ViewBag.ProductId = productId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductVariant variant)
        {
            _db.ProductVariants.Add(variant);
            _db.SaveChanges();
            return RedirectToAction("Manage", new { productId = variant.ProductId });
        }

        public IActionResult Edit(int id)
        {
            var variant = _db.ProductVariants.Find(id);
            if (variant == null) return NotFound();
            return View(variant);
        }

        [HttpPost]
        public IActionResult Edit(ProductVariant variant)
        {
            _db.ProductVariants.Update(variant);
            _db.SaveChanges();
            return RedirectToAction("Manage", new { productId = variant.ProductId });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var variant = _db.ProductVariants.Find(id);
            if (variant == null) return NotFound();
            var productId = variant.ProductId;
            _db.ProductVariants.Remove(variant);
            _db.SaveChanges();
            return RedirectToAction("Manage", new { productId });
        }
    }
}