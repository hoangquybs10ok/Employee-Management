namespace EmployeeManagement.EF.Entity
{
    public class CategoryEntity : EntityBase
    {
        public string? Name { get; set; }
        public ICollection<ProductCategoryEntity> Products { get; set; }=new List<ProductCategoryEntity>();
    }
}
