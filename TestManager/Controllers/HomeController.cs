using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestManager.Models;

namespace TestManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(ModelBinderAttribute del)
        {
            var secsion = HttpContext.Session.GetString("UserName");
            if (secsion == null)
            {
                return RedirectToAction("Login", "Account");
            }


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
