using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var sessionUser = HttpContext.Session.GetString("UserName");
            if (sessionUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.FullName = HttpContext.Session.GetString("FullName");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
