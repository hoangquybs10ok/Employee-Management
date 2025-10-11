using EmployeeManagement.EF.Entity.Enums;
using EmployeeManagement.EF.Models;

namespace EmployeeManagement.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public List<OrderModel> Orders { get; set; } = new List<OrderModel>();
        public List<TimeLogModel> TimeLogs { get; set; } = new List<TimeLogModel>();
        public List<int>? ProjectIds { get; set; }
        public List<ProjectModel> Projects { get; set; } = new List<ProjectModel>();
        public RoleType Role { get; set; }
    }
}
