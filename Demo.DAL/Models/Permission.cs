using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.DAL.Models
{
    [Table("Permissions")]
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

        [StringLength(100)]
        public string Page { get; set; }

        [Display(Name = "Can View")]
        public bool? CanView { get; set; }

        [Display(Name = "Can Edit")]
        public bool? CanEdit { get; set; }

        [Display(Name = "Can Delete")]
        public bool? CanDelete { get; set; }
    }
}