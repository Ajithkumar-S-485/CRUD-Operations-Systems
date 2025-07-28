using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.DAL.Models
{
    [Table("Vouchers")]
    public class Voucher
    {
        [Key]
        public int VoucherId { get; set; }

        [StringLength(50)]
        [Display(Name = "Voucher Type")]
        public string VoucherType { get; set; }

        [Required]
        [Display(Name = "Voucher Date")]
        [DataType(DataType.Date)]
        public DateTime VoucherDate { get; set; }

        [Display(Name = "Member")]
        public int? MemberId { get; set; }

        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Amount { get; set; }

        [Required]
        [Display(Name = "Account")]
        public int AccountId { get; set; }

        [Required]
        [Display(Name = "Journal Entry")]
        public int JournalId { get; set; }

        [StringLength(255)]
        public string Remarks { get; set; }

        // Navigation properties
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        [ForeignKey("AccountId")]
        public virtual LedgerAccount LedgerAccount { get; set; }

        [ForeignKey("JournalId")]
        public virtual JournalEntry JournalEntry { get; set; }
    }
}