using Microsoft.AspNetCore.Mvc; // dùng cho controller, viewbag, actionresult
using EmployeeManagement.EF.Entity.Enums; // chứa enum RoleType, admin, manager, hr, employee
using Microsoft.AspNetCore.Authorization; // để dùng [Authorize]
using System.Security.Claims; // đọc thông tin từ cookie authentication như ( "user name ", "role", "full name")
using Microsoft.AspNetCore.Mvc.Filters; // để dùng OnActionExecuted (hook sau khi action dc thực thi)
namespace EmployeeManagement.Controllers
{
    [Authorize] // yêu cầu người dùng đã xác thực (đăng nhập) mới được truy cập các action trong controller nào kế thừa basecontroller.
                // Nếu chưa đăng nhập, họ sẽ bị chuyển hướng đến trang đăng nhập account/login.
    public class BaseController : Controller
    {
        protected string? CurrentUserName => HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        protected string? CurrentFullName => HttpContext.User.FindFirst("FullName")?.Value;
        protected string? CurrentRole => HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        protected bool IsAdmin() => CurrentRole == RoleType.Admin.ToString();
        protected bool IsManager() => CurrentRole == RoleType.Manager.ToString();
        protected bool IsHR() => CurrentRole == RoleType.HR.ToString();
        protected bool IsEmployee() => CurrentRole == RoleType.Employee.ToString();
        protected bool IsMember() => IsEmployee() || IsManager() || IsHR();
        public override void OnActionExecuted(Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext context)
        {
            ViewBag.CurrentUserName = CurrentUserName;
            ViewBag.CurrentFullName = CurrentFullName;
            ViewBag.CurrentRole = CurrentRole;
            base.OnActionExecuted(context);
        }
    }
}
