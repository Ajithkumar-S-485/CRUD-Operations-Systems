using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Code Is Required")]
        public string Code { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public DateTime DateOfCreation { get; set; }
    }

    public class CreateDepartmentDto
    {
        [Required(ErrorMessage = "Code Is Required")]
        public string Code { get; set; }
        
        [Required]
        public string Name { get; set; }
    }

    public class UpdateDepartmentDto
    {
        [Required(ErrorMessage = "Code Is Required")]
        public string Code { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}