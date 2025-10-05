using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Controllers;
using EmployeeManagement.Entity;

namespace EmployeeManagement.TestDb
{
    public class DbContextTest :DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<TimeLogEntity> TimeLogs { get; set; }
        public DbSet<OrderProductEntity> OrderProducts { get; set; }
        public DbSet<ProductCategoryEntity> ProductCategories { get; set; }
        public DbSet<UserProjectEntity> UserProjects { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=EmployeeManagementment;user=root;password=1234@Abcd",
                new MySqlServerVersion(new Version(8, 0, 29)));
        }
    }
}
