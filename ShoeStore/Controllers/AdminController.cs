using Microsoft.AspNetCore.Mvc;
using ShoeStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ShoeStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminController(ApplicationDbContext db) => _db = db;

        private const string RootEmail = "admin@shop.com";
        private bool IsAdmin() => HttpContext.Session.GetString("UserRole") == "Admin";
        private bool IsRootAdmin() => HttpContext.Session.GetString("UserEmail") == RootEmail; 

        public async Task<IActionResult> Users()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            // Gửi RootEmail sang View để ẩn/hiện nút cho chuẩn
            ViewBag.RootEmail = RootEmail;

            var users = await _db.Users.OrderBy(u => u.Role).ThenBy(u => u.FullName).ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(int userId, string newRole)
        {
            // Bảo mật tầng 1: Phải là Admin
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            // Bảo mật tầng 2: Chỉ thằng ID = 1 mới được quyền đổi
            if (!IsRootAdmin()) // Kiểm tra bằng Email ở hàm trên
            {
                TempData["Error"] = "Bạn không có quyền này!";
                return RedirectToAction("Users");
            }

            var userToChange = await _db.Users.FindAsync(userId);
            if (userToChange == null) return NotFound();

            // Bảo mật tầng 3: Không cho ai đụng vào chính thằng ID 1
            if (userToChange.Email == RootEmail)
            {
                TempData["Error"] = "Không thể đổi tài khoản Root Admin!";
                return RedirectToAction("Users");
            }

            // Không cho tự đổi của chính mình
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
            TempData["Success"] = $"Đã ném {user.FullName} vào thùng rác";
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