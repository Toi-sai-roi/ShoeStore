using Microsoft.AspNetCore.Mvc;
using ShoeStore.Data;
using Microsoft.EntityFrameworkCore;

namespace ShoeStore.Controllers
{
    [AdminOnly]
    public class AdminController : BaseController
    {
        private readonly ApplicationDbContext _db;
        public AdminController(ApplicationDbContext db) => _db = db;

        private const string RootEmail = "admin@shop.com";
        private bool IsRootAdmin() => HttpContext.Session.GetString("UserEmail") == RootEmail;

        public async Task<IActionResult> Users()
        {
            ViewBag.RootEmail = RootEmail;
            var users = await _db.Users.OrderBy(u => u.Role).ThenBy(u => u.FullName).ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(int userId, string newRole)
        {
            if (!IsRootAdmin())
            {
                TempData["Error"] = "Bạn không có quyền này!";
                return RedirectToAction("Users");
            }

            var userToChange = await _db.Users.FindAsync(userId);
            if (userToChange == null) return NotFound();

            if (userToChange.Email == RootEmail)
            {
                TempData["Error"] = "Không thể đổi tài khoản Root Admin!";
                return RedirectToAction("Users");
            }

            var currentEmail = HttpContext.Session.GetString("UserEmail");
            if (userToChange.Email == currentEmail)
            {
                TempData["Error"] = "Bạn không thể tự đổi vai trò của mình!";
                return RedirectToAction("Users");
            }

            userToChange.Role = newRole;
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Đã đổi {userToChange.FullName} sang {newRole} thành công!";
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> SoftDeleteUser(int userId)
        {
            if (!IsRootAdmin())
            {
                TempData["Error"] = "Chỉ Root Admin mới xóa được!";
                return RedirectToAction("Users");
            }

            var user = await _db.Users.FindAsync(userId);
            if (user == null) return NotFound();

            if (user.Email == RootEmail)
            {
                TempData["Error"] = "Không thể xóa Root Admin!";
                return RedirectToAction("Users");
            }

            user.IsDeleted = true;
            user.DeletedAt = DateTime.Now;
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Đã xóa tài khoản {user.FullName}";
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> RestoreUser(int userId)
        {
            if (!IsRootAdmin()) return RedirectToAction("Users");

            var user = await _db.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.IsDeleted = false;
            user.DeletedAt = null;
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Đã khôi phục {user.FullName}";
            return RedirectToAction("Users");
        }
    }
}