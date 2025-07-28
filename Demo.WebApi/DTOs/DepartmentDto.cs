using System.ComponentModel.DataAnnotations;

namespace Demo.WebApi.DTOs
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        
        public DateTime DateOfCreation { get; set; }
        
        public int EmployeeCount { get; set; }
    }

    public class CreateDepartmentDto
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateDepartmentDto
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
    }
}