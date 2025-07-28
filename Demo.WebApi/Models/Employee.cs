using System.ComponentModel.DataAnnotations;

namespace Demo.WebApi.Models
{
    public class Employee : ModelBase
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
        
        public bool IsDeleted { get; set; } = false;
        
        public DateTime CreationDate { get; set; } = DateTime.Now;
        
        public string ImageName { get; set; } = string.Empty;
        
        public int? DepartmentId { get; set; } // Foreign Key Column
        
        public Department? Department { get; set; }
    }
}