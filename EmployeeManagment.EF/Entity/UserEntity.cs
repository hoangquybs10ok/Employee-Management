
using EmployeeManagement.EF.Entity.Enums;

namespace EmployeeManagement.EF.Entity
{
    public class UserEntity : EntityBase
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public RoleType Role { get; set; }
        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
        public ICollection<TimeLogEntity> TimeLogs { get; set; } = new List<TimeLogEntity>();
        public ICollection<UserProjectEntity> UserProjects { get; set; } = new List<UserProjectEntity>();
    }
}
