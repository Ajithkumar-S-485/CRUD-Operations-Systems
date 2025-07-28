using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.DAL.Models
{
    [Table("Members")]
    public class Member
    {
        [Key]
        public int MemberId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [StringLength(15)]
        [Display(Name = "Mobile Number")]
        public string Mobile { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Join Date")]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Fee> Fees { get; set; }
        public virtual ICollection<MedicaidRequest> MedicaidRequests { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}