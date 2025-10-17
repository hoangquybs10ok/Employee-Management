using EmployeeManagement.Model;
using EmployeeManagement.Service;
using System.Net;
using System.Net.Mail;

namespace EmployeeManagement.Services
{
    public class EmailService : IEmailservice
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendMail(EmailSendModel emailSendModel)
        {
            // Kiểm tra dữ liệu đầu vào
            if (emailSendModel == null)
                throw new ArgumentNullException(nameof(emailSendModel), "EmailSendModel không được null.");

            if (string.IsNullOrWhiteSpace(emailSendModel.To))
                throw new ArgumentException("Email người nhận không được để trống hoặc sai định dạng.", nameof(emailSendModel.To));

            if (string.IsNullOrWhiteSpace(emailSendModel.Subject))
                emailSendModel.Subject = "(Không có tiêu đề)";

            if (string.IsNullOrWhiteSpace(emailSendModel.Body))
                emailSendModel.Body = "(Không có nội dung)";

            // Lấy thông tin cấu hình từ appsettings.json
            var smtpServer = _config["EmailSettings:SmtpServer"];
            var port = int.Parse(_config["EmailSettings:Port"]!);
            var senderEmail = _config["EmailSettings:SenderEmail"];
            var senderName = _config["EmailSettings:SenderName"];
            var password = _config["EmailSettings:Password"];

            // Kiểm tra lại các cấu hình email
            if (string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(password))
                throw new Exception("Cấu hình EmailSettings trong appsettings.json chưa đầy đủ.");

            //  Tạo email
            var message = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = emailSendModel.Subject,
                Body = emailSendModel.Body,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(emailSendModel.To));

            //  Gửi qua SMTP
            using (var smtp = new SmtpClient(smtpServer, port))
            {
                smtp.Credentials = new NetworkCredential(senderEmail, password);
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(message);
                    Console.WriteLine($" Gửi email thành công tới: {emailSendModel.To}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Lỗi khi gửi email: {ex.Message}");
                    throw;
                }
            }
        }

    }
}
