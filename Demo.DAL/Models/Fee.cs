using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.DAL.Models
{
    [Table("Fees")]
    public class Fee
    {
        [Key]
        public int FeeId { get; set; }

        [Required]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

        [Range(1, 12)]
        [Display(Name = "Fee Month")]
        public int? FeeMonth { get; set; }

        [Display(Name = "Fee Year")]
        public int? FeeYear { get; set; }

        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Amount { get; set; }

        [Display(Name = "Paid Date")]
        [DataType(DataType.Date)]
        public DateTime? PaidDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Receipt Number")]
        public string ReceiptNumber { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        // Navigation properties
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }
    }
}