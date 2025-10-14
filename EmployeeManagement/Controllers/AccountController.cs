using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;
using EmployeeManagement.EF.TestDb;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using EmployeeManagement.EF.Entity.Enums;

namespace EmployeeManagement.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly DbContextTest _context;
        public AccountController(DbContextTest context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            //  Tìm user trong DB
            var user = _context.Users.FirstOrDefault(u => u.UserName == model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu!");
                return View(model);
            }

            // Kiểm tra password (BCrypt)
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu!");
                return View(model);
            }

            //  Tạo danh sách claim
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim("FullName", user.FullName ?? ""),
                new Claim("Email", user.Email ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString() ?? "User") 
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //  Cấu hình cookie
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            //  Đăng nhập (ghi cookie)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            //  Chuyển đến trang User
            // Chuyển hướng sau khi đăng nhập
            if (user.Role == RoleType.Employee)
            {
                return RedirectToAction("Index", "TimeLog");
            }
            else
            {
                return RedirectToAction("Index", "User");
            }

        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet("accessdenied")]
        [AllowAnonymous]
        public IActionResult AccessDenied() => View();
    }
}
