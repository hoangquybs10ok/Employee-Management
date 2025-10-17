using EmployeeManagement.Model;

namespace EmployeeManagement.Service
{
    public interface IEmailservice
    {
        void SendMail(EmailSendModel emailSendModel);
    }
}
