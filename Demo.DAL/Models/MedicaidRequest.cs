using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.DAL.Models
{
    [Table("MedicaidRequests")]
    public class MedicaidRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

        [Required]
        [Display(Name = "Request Date")]
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; }

        [Display(Name = "Amount Requested")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? AmountRequested { get; set; }

        [Display(Name = "Approved Amount")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? ApprovedAmount { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        [StringLength(255)]
        public string Description { get; set; }

        // Navigation properties
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }
    }
}