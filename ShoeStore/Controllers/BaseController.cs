using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShoeStore.Controllers
{
    public class BaseController : Controller
    {
        protected bool IsAdmin() => HttpContext.Session.GetString("UserRole") == "Admin";
        protected bool IsAdminOrStaff() => IsAdmin() || HttpContext.Session.GetString("UserRole") == "Staff";
        protected int? CurrentUserId => HttpContext.Session.GetInt32("UserId");
    }

    public class AdminOnlyAttribute : TypeFilterAttribute
    {
        public AdminOnlyAttribute() : base(typeof(AdminOnlyFilter)) { }
    }

    public class AdminOrStaffAttribute : TypeFilterAttribute
    {
        public AdminOrStaffAttribute() : base(typeof(AdminOrStaffFilter)) { }
    }

    public class AdminOnlyFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
                context.Result = new RedirectToActionResult("Login", "Account", null);
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }

    public class AdminOrStaffFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("UserRole");
            if (role != "Admin" && role != "Staff")
                context.Result = new RedirectToActionResult("Index", "Home", null);
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}