using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;
using EmployeeManagement.EF.TestDb;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

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
            //if (HttpContext.Session.GetString("UserName") != null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users.FirstOrDefault(u => u.UserName == model.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Sai tài khoản hoặc mật khẩu!");
                return View(model);
            }
      
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim("FullName", user.FullName ?? ""),
                new Claim("Email", user.Email ?? ""),
                new Claim(ClaimTypes.Role, user.Role.ToString() ?? "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "User");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (Request.Cookies.ContainsKey("AuthToken"))
            {
                Response.Cookies.Delete("AuthToken");
            }

            return RedirectToAction("Login");
        }

        [HttpGet("accessdenied")]
        public IActionResult AccessDenied() => View();
    }
}
