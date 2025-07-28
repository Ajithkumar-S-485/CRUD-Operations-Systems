using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.DAL.Models
{
    [Table("LedgerAccounts")]
    public class LedgerAccount
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [StringLength(50)]
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [Display(Name = "Is Cash Account")]
        public bool IsCashAccount { get; set; } = false;

        // Navigation properties
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}