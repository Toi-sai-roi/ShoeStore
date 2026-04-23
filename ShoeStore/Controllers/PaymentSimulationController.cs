using Microsoft.AspNetCore.Mvc;
using ShoeStore.Data;

namespace ShoeStore.Controllers
{
    public class PaymentSimulationController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PaymentSimulationController(ApplicationDbContext db) => _db = db;

        // Trang điều hướng giả lập
        public IActionResult Process(int orderId, string method)
        {
            var order = _db.Orders.Find(orderId);
            if (order == null) return NotFound();

            ViewBag.Method = method; // "MoMo", "VNPay", "ZaloPay", "PayPal"
            return View(order);
        }

        // Sau khi bấm "Xác nhận trả tiền" trên giao diện giả
        [HttpPost]
        public IActionResult ConfirmPayment(int orderId)
        {
            var order = _db.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = "Confirmed"; // Đánh dấu đã thanh toán
                _db.SaveChanges();
            }
            return RedirectToAction("Success", "Order", new { id = orderId });
        }
    }
}