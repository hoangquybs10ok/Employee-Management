namespace TestManager.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
    }
}
