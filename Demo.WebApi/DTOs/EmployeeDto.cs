using System.ComponentModel.DataAnnotations;

namespace Demo.WebApi.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? Age { get; set; }
        public string Address { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime HiringDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }

    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        
        public int? Age { get; set; }
        
        public string Address { get; set; } = string.Empty;
        
        [Required]
        public decimal Salary { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        
        public DateTime HiringDate { get; set; } = DateTime.Now;
        
        public string ImageName { get; set; } = string.Empty;
        
        public int? DepartmentId { get; set; }
    }

    public class UpdateEmployeeDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        
        public int? Age { get; set; }
        
        public string Address { get; set; } = string.Empty;
        
        [Required]
        public decimal Salary { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        
        public DateTime HiringDate { get; set; }
        
        public string ImageName { get; set; } = string.Empty;
        
        public int? DepartmentId { get; set; }
    }
}