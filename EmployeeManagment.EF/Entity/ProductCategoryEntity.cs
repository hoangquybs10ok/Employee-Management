
namespace EmployeeManagement.EF.Entity
{
    public class ProductCategoryEntity : EntityBase
    {
        public int ProductId {  get; set; }
        public ProductEntity? Product { get; set; }
        public int CategoryId { get; set; }
        public CategoryEntity? Category { get; set; }
    }
}
