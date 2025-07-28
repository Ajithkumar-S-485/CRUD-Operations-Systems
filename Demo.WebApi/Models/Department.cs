using System.ComponentModel.DataAnnotations;

namespace Demo.WebApi.Models
{
    public class Department : ModelBase
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        
        [Display(Name = "Date Of Creation")]
        public DateTime DateOfCreation { get; set; } = DateTime.Now;
        
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}