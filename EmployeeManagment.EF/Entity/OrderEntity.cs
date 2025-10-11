using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.EF.Entity
{
    public class OrderEntity : EntityBase
    {
        public DateTime? OrderDate { get; set; }
        public int UserId { get; set; }
        public UserEntity? User { get; set; }
        public ICollection<OrderProductEntity> Products { get; set; } = new List<OrderProductEntity>();
    }
}
