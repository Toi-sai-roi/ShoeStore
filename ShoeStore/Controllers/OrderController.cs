using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Data;
using ShoeStore.Models;

namespace ShoeStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public OrderController(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        private bool IsAdmin() => HttpContext.Session.GetString("UserRole") == "Admin";
        private bool IsAdminOrStaff() => IsAdmin() || HttpContext.Session.GetString("UserRole") == "Staff";

        // ================= USER FUNCTIONS =================


        [HttpGet]
        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var cart = _db.CartItems
                          .Where(c => c.UserId == userId)
                          .Include(c => c.Variant).ThenInclude(v => v!.Product)
                          .ToList();

            if (!cart.Any()) return RedirectToAction("Index", "Cart");

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            ViewBag.FullName = user?.FullName;
            ViewBag.Phone = user?.Phone;
            ViewBag.Address = user?.Address;

            return View(cart);
        }

        [HttpPost]
        public IActionResult Checkout(string customerName, string? customerPhone, string? customerAddress, string paymentMethod = "COD")
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var cart = _db.CartItems
                          .Where(c => c.UserId == userId)
                          .Include(c => c.Variant)
                          .ThenInclude(v => v!.Product)
                          .ToList();

            if (!cart.Any()) return RedirectToAction("Index", "Cart");

            try
            {
                // TÍNH TỔNG TIỀN TRỰC TIẾP (Fix lỗi bằng 0)
                decimal total = cart.Sum(i => i.Quantity * i.UnitPrice);

                var order = new Order
                {
                    UserId = userId,
                    CustomerName = customerName,
                    CustomerPhone = customerPhone,
                    CustomerAddress = customerAddress,
                    PaymentMethod = paymentMethod,
                    TotalAmount = total,
                    CreatedAt = DateTime.Now,
                    // Nếu Online thì chờ thanh toán (Pending), nếu COD thì duyệt luôn (Confirmed)
                    Status = (paymentMethod == "COD") ? "Confirmed" : "Pending"
                };

                foreach (var item in cart)
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        VariantId = item.VariantId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    });
                }

                _db.Orders.Add(order);
                _db.CartItems.RemoveRange(cart); // Dọn giỏ hàng
                _db.SaveChanges();

                var onlineMethods = new List<string> { "MoMo", "ZaloPay", "VNPay", "PayPal" };
                if (onlineMethods.Contains(paymentMethod))
                {
                    return RedirectToAction("Process", "PaymentSimulation", new { orderId = order.Id, method = paymentMethod });
                }

                return RedirectToAction("Success", new { id = order.Id });
            }
            catch (Exception ex)
            {
                return Content("Lỗi xử lý đơn hàng: " + ex.Message);
            }
        }

        public IActionResult Success(int id)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        public IActionResult History()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var orders = _db.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Variant)
                .ThenInclude(v => v!.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();
            // Logic lừa thị giác cho User (không save vào DB để demo liên tục được)
            foreach (var order in orders)
            {
                var secondsSinceCreated = (DateTime.Now - order.CreatedAt).TotalSeconds;

                if (order.Status == "Confirmed")
                {
                    if (secondsSinceCreated >= 60) order.Status = "Delivered";
                    else if (secondsSinceCreated >= 30) order.Status = "Delivering";
                }

                if (order.Status == "Pending" && secondsSinceCreated >= 30)
                {
                    order.Status = "Cancelled";
                }
            }

            return View(orders);
        }

        // ================= ADMIN FUNCTIONS =================

        public IActionResult AdminIndex(string? search, string? status)
        {
            if (!IsAdminOrStaff()) return RedirectToAction("Index", "Home");

            var query = _db.Orders.Include(o => o.User).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(o => o.CustomerName.Contains(search) || o.User!.Email.Contains(search));

            if (!string.IsNullOrEmpty(status))
                query = query.Where(o => o.Status == status);

            var orders = query.OrderByDescending(o => o.CreatedAt).ToList();

            bool hasChanges = false;
            foreach (var order in orders)
            {
                var secondsSinceCreated = (DateTime.Now - order.CreatedAt).TotalSeconds;

                if (order.Status == "Pending" && secondsSinceCreated >= 30)
                {
                    order.Status = "Cancelled";
                    hasChanges = true;
                }

                if (order.Status == "Confirmed")
                {
                    if (secondsSinceCreated >= 60) { order.Status = "Delivered"; hasChanges = true; }
                    else if (secondsSinceCreated >= 30) { order.Status = "Delivering"; hasChanges = true; }
                }
            }

            if (hasChanges) _db.SaveChanges();

            ViewBag.Search = search;
            ViewBag.Status = status;

            return View(orders);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int orderId, string status)
        {
            if (!IsAdminOrStaff()) return Forbid();

            var order = _db.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = status;
                // Reset thời gian để Admin có 30s "yên tĩnh" quan sát trạng thái vừa chọn
                order.CreatedAt = DateTime.Now;
                _db.SaveChanges();
            }
            return RedirectToAction("AdminIndex");
        }

        public IActionResult Revenue()
        {
            if (!IsAdminOrStaff()) return RedirectToAction("Index", "Home");

            var orders = _db.Orders
                            .Where(o => o.Status == "Delivered" || o.Status == "Confirmed")
                            .Include(o => o.OrderDetails)
                                .ThenInclude(d => d.Variant)
                                    .ThenInclude(v => v!.Product)
                            .ToList();

            ViewBag.TotalRevenue = orders.Sum(o => o.TotalAmount);
            ViewBag.TotalOrders = orders.Count;
            ViewBag.TopProducts = orders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(d => d.Variant?.Product?.Name ?? "Unknown")
                .Select(g => new {
                    Name = g.Key,
                    Quantity = g.Sum(d => d.Quantity),
                    Revenue = g.Sum(d => d.UnitPrice * d.Quantity)
                })
                .OrderByDescending(x => x.Revenue)
                .Take(5)
                .ToList();

            return View();
        }
    }
}