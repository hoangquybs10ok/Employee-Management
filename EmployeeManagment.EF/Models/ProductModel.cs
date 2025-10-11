namespace EmployeeManagement.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
        public ICollection<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
    }
}
