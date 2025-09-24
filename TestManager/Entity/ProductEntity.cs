using Microsoft.EntityFrameworkCore.Storage;

namespace TestManager.Entity
{
    public class ProductEntity : EntityBase
    {
        public string? ProductName { get; set; }
        public decimal Price {  get; set; }
        public string? Description { get; set; }
        public ICollection<OrderProductEntity> Orders { get; set; } = new List<OrderProductEntity>();
        public ICollection<ProductCategoryEntity> Categories { get; set; } = new List<ProductCategoryEntity>();
    }
}
