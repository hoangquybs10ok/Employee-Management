using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.EF.Repository.Interface;
using EmployeeManagement.Models;
using EmployeeManagement.Service;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmailservice _emailservice;
        private ITimeLogRepository _timeLogRepository;

        public HomeController(IEmailservice emailservice, ITimeLogRepository timeLogRepository)
        {
            _emailservice = emailservice;
            _timeLogRepository = timeLogRepository;
        }
        public IActionResult Index()
        {
            var emailModel = new Model.EmailSendModel
            {
                To = "hoangquyhotboy@gmail.com", // Email người nhận
                Subject = "Test Email từ hệ thống Employee Management",
                Body = "<h3>Xin chào!</h3>" +
                "<p>Email này được gửi thành công từ hệ thống.</p>"
            };

            _emailservice.SendMail(emailModel);

            ViewBag.FullName = User.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
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
