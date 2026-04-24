using Microsoft.AspNetCore.Mvc;
using ShoeStore.Data;
using ShoeStore.Models;

namespace ShoeStore.Controllers
{
    [AdminOrStaff]
    public class CategoryController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db) => _db = db;

        // GET: Lấy danh sách danh mục
        public IActionResult Index()
        {
            var categories = _db.Categories.ToList();
            return View(categories);
        }

        // GET: Form tạo danh mục
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tạo danh mục mới (AJAX)
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                return Json(new { success = false, message = "Tên danh mục không được trống" });
            }

            // Kiểm tra danh mục đã tồn tại chưa
            var existingCategory = _db.Categories.FirstOrDefault(c => c.Name.ToLower() == category.Name.ToLower());
            if (existingCategory != null)
            {
                return Json(new { success = false, message = "Danh mục này đã tồn tại" });
            }

            try
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                return Json(new { success = true, categoryId = category.Id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        // GET: Form sửa danh mục
        public IActionResult Edit(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: Cập nhật danh mục
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError("Name", "Tên danh mục không được trống");
                return View(category);
            }

            var existingCategory = _db.Categories.FirstOrDefault(c => c.Name.ToLower() == category.Name.ToLower() && c.Id != category.Id);
            if (existingCategory != null)
            {
                ModelState.AddModelError("Name", "Danh mục này đã tồn tại");
                return View(category);
            }

            try
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["Success"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
                return View(category);
            }
        }

        // POST: Xóa danh mục
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null) return NotFound();

            // Kiểm tra xem danh mục có sản phẩm không
            var hasProducts = _db.Products.Any(p => p.CategoryId == id);
            if (hasProducts)
            {
                TempData["Error"] = "Không thể xóa danh mục này vì đang có sản phẩm sử dụng";
                return RedirectToAction("Index");
            }

            try
            {
                _db.Categories.Remove(category);
                _db.SaveChanges();
                TempData["Success"] = "Xóa danh mục thành công";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}