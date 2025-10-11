using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vui lòng nhập UserName")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; } = string.Empty;
    }
}
