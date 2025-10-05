using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Entity
{
    public class OrderProductEntity : EntityBase
    {
        public int OrderId {  get; set; }
        public OrderEntity? Order { get; set; }
        public int ProductId { get; set; }
        public ProductEntity? Product { get; set; }
    }
}
