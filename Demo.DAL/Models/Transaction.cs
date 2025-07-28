using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.DAL.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        [Display(Name = "Journal Entry")]
        public int JournalId { get; set; }

        [Required]
        [Display(Name = "Account")]
        public int AccountId { get; set; }

        [Display(Name = "Debit Amount")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal DebitAmount { get; set; } = 0;

        [Display(Name = "Credit Amount")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal CreditAmount { get; set; } = 0;

        // Navigation properties
        [ForeignKey("JournalId")]
        public virtual JournalEntry JournalEntry { get; set; }

        [ForeignKey("AccountId")]
        public virtual LedgerAccount LedgerAccount { get; set; }
    }
}