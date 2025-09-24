using Microsoft.EntityFrameworkCore;
using TestManager.Entity;

namespace TestManager.TestDb
{
    public class DbContextTest :DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<TimeLogEntity> TimeLogs { get; set; }
        public DbSet<OrderProductEntity> OrderProducts { get; set; }
        public DbSet<ProductCategoryEntity> ProductCategories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=productmanager;user=root;password=1234@Abcd",
                new MySqlServerVersion(new Version(8, 0, 29)));
        }
    }
}
