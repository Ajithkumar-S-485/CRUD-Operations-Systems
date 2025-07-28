using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.DAL.Models
{
    [Table("CashBook")]
    public class CashBook
    {
        [Key]
        public int CashBookId { get; set; }

        [Required]
        [Display(Name = "Entry Date")]
        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Display(Name = "Debit")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Debit { get; set; } = 0;

        [Display(Name = "Credit")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Credit { get; set; } = 0;

        [Display(Name = "Balance")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Balance { get; set; }
    }
}