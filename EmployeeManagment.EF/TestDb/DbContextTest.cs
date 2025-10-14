using Microsoft.EntityFrameworkCore;
using EmployeeManagement.EF.Entity;

namespace EmployeeManagement.EF.TestDb
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
            optionsBuilder.UseMySql("server=localhost;database=employeemanagement;user=root;password=1234@Abcd",
                new MySqlServerVersion(new Version(8, 0, 29)));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ giữa User và TimeLog
            modelBuilder.Entity<TimeLogEntity>()
                .HasOne(t => t.User)
                .WithMany(u => u.TimeLogs)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
