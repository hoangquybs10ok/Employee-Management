using Microsoft.AspNetCore.Mvc;
using TestManager.Models;
using TestManager.TestDb;

namespace TestManager.Controllers
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
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = _context.Users.FirstOrDefault(u => u.UserName == model.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Sai tai khoan hoac mat khau!");
                return View(model);
            }
            if (!string.IsNullOrEmpty(user.UserName))
            {
                HttpContext.Session.SetString("UserName", user.UserName);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
