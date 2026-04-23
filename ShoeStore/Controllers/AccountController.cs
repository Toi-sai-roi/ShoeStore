using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Data;
using ShoeStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies; 
using Microsoft.AspNetCore.Authentication;

namespace ShoeStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        // Tự động restore session từ cookie
        private void RestoreSessionFromCookie()
        {
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
        }

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string? phone, string? address)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                ViewBag.Error = "Email đã tồn tại.";
                return View();
            }

            var user = new User
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Phone = phone,
                Address = address,
                Role = "Customer"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }

        // GET: /Account/Login
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng.";
                return View();
            }

            // Session vẫn giữ
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role);

            // Thêm Cookie
            var claims = new List<System.Security.Claims.Claim>
            {
                new("UserId", user.Id.ToString()),
                new("UserEmail", user.Email),
                new("UserRole", user.Role)
            };
            var identity = new System.Security.Claims.ClaimsIdentity(claims, "CookieAuth");
            var principal = new System.Security.Claims.ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("CookieAuth", principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            });

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }

        // GET: /Account/Profile
        public IActionResult Profile()
        {
            RestoreSessionFromCookie();
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return RedirectToAction("Login");

            return View(user);
        }

        // POST: /Account/Profile
        [HttpPost]
        public async Task<IActionResult> Profile(string fullName, string phone, string address)
        {
            RestoreSessionFromCookie();
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return RedirectToAction("Login");

            user.FullName = fullName;
            user.Phone = phone;
            user.Address = address;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Profile");
        }
    }
}