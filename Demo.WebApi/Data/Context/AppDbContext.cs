using Demo.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.WebApi.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Code).IsRequired().HasMaxLength(50);
                entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(d => d.Code).IsUnique();
            });

            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.ImageName).HasMaxLength(100);
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
                
                // Configure foreign key relationship
                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Seed data
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Code = "IT", Name = "Information Technology", DateOfCreation = DateTime.Now },
                new Department { Id = 2, Code = "HR", Name = "Human Resources", DateOfCreation = DateTime.Now },
                new Department { Id = 3, Code = "FIN", Name = "Finance", DateOfCreation = DateTime.Now }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee 
                { 
                    Id = 1, 
                    Name = "John Doe", 
                    Age = 30, 
                    Address = "123 Main St", 
                    Salary = 75000, 
                    Email = "john.doe@company.com", 
                    PhoneNumber = "555-0123",
                    HiringDate = DateTime.Now.AddYears(-2),
                    DepartmentId = 1,
                    IsActive = true,
                    CreationDate = DateTime.Now
                },
                new Employee 
                { 
                    Id = 2, 
                    Name = "Jane Smith", 
                    Age = 28, 
                    Address = "456 Oak Ave", 
                    Salary = 65000, 
                    Email = "jane.smith@company.com", 
                    PhoneNumber = "555-0124",
                    HiringDate = DateTime.Now.AddYears(-1),
                    DepartmentId = 2,
                    IsActive = true,
                    CreationDate = DateTime.Now
                }
            );
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}