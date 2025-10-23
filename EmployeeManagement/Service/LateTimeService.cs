using EmployeeManagement.EF.TestDb;
using EmployeeManagement.Model;
using EmployeeManagement.Service;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace EmployeeManagement.Services
{
    public class LateTimeService : ILateTimeService
    {
        private readonly DbContextTest _context;
        private readonly IEmailservice _emailService;

        public LateTimeService(DbContextTest context, IEmailservice emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task SendYesterdayTimeLogReportAsync()
        {
            var yesterday = DateTime.Today.AddDays(-1);

            // Lấy dữ liệu TimeLog hôm qua
            var logs = await _context.TimeLogs
                .Include(t => t.User)
                .Where(t => t.CheckIn.Date == yesterday.Date)
                .OrderBy(t => t.User!.FullName)
                .ToListAsync();

            if (!logs.Any())
            {
                Console.WriteLine("Không có dữ liệu TimeLog ngày hôm qua.");
                return;
            }

            // Tạo nội dung HTML cho email
            var sb = new StringBuilder();
            sb.Append("<h3>BÁO CÁO GIỜ LÀM VIỆC NGÀY HÔM QUA</h3>");
            sb.Append("<table border='1' cellspacing='0' cellpadding='5'>");
            sb.Append("<tr><th>Tên nhân viên</th><th>Giờ Check-in</th><th>Giờ Check-out</th><th>Công việc</th></tr>");

            foreach (var log in logs)
            {
                sb.Append($"<tr>");
                sb.Append($"<td>{log.User?.FullName}</td>");
                sb.Append($"<td>{log.CheckIn:HH:mm}</td>");
                sb.Append($"<td>{(log.CheckOut.HasValue ? log.CheckOut.Value.ToString("HH:mm") : "Chưa check-out")}</td>");
                sb.Append($"<td>{log.Task ?? "Không có"}</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");

            var admin = _context.Users.Where(x => x.Role == EF.Entity.Enums.RoleType.Admin)
                .ToList();

            foreach (var user in admin)
            {
                var email = new EmailSendModel
                {
                    To = user.Email!,
                    Subject = $"Báo cáo giờ làm việc ngày {yesterday:dd/MM/yyyy}",
                    Body = sb.ToString()
                };

                _emailService.SendMail(email);
            }

        }
    }
}
