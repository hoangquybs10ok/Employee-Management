using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.EF.Entity.Enums;

namespace EmployeeManagement.Controllers
{
    public class BaseController :Controller
    {
        protected bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == RoleType.Admin.ToString();
        }
        protected bool IsManager()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == RoleType.Manager.ToString();
        }
        protected bool IsHR()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == RoleType.HR.ToString();
        }
        protected bool IsEmployee()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == RoleType.Employee.ToString();
        }
        protected bool IsMember()
        {
            return IsManager () || IsHR() || IsEmployee();
        }
    }
}
