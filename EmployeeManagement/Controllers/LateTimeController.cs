using EmployeeManagement.EF.TestDb;
using EmployeeManagement.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers
{
    [Route("latetime")]
    public class LateTimeController : Controller
    {
        private readonly DbContextTest _context;
        private readonly ILateTimeService _lateTimeService;

        public LateTimeController(DbContextTest context, ILateTimeService lateTimeService)
        {
            _context = context;
            _lateTimeService = lateTimeService;
        }

        // Trang Index hiển thị TimeLog hôm qua
        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var yesterday = DateTime.Today.AddDays(-1);

            var logs = await _context.TimeLogs
                .Include(t => t.User)
                .Where(t => t.CheckIn.Date == yesterday.Date)
                .OrderBy(t => t.User!.FullName)
                .ToListAsync();

            ViewBag.Yesterday = yesterday.ToString("dd/MM/yyyy");
            return View(logs);
        }

        // Nút gửi báo cáo
        [HttpPost("send-report")]
        public async Task<IActionResult> SendReport()
        {
            await _lateTimeService.SendYesterdayTimeLogReportAsync();
            TempData["Message"] = " Báo cáo giờ làm việc hôm qua đã được gửi email thành công!";
            return RedirectToAction("Index");
        }
    }
}
