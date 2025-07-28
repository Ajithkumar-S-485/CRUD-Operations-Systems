using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.DAL.Models
{
    [Table("JournalEntries")]
    public class JournalEntry
    {
        [Key]
        public int JournalId { get; set; }

        [Required]
        [Display(Name = "Entry Date")]
        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; }

        [StringLength(255)]
        public string Narration { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        // Navigation properties
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}