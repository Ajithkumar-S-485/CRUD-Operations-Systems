using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.DAL.Models
{
    [Table("Events")]
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Required]
        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Display(Name = "Budget Amount")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? BudgetAmount { get; set; }

        [Display(Name = "Actual Expense")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? ActualExpense { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [NotMapped]
        [Display(Name = "Budget Variance")]
        public decimal? BudgetVariance => BudgetAmount.HasValue && ActualExpense.HasValue 
            ? BudgetAmount.Value - ActualExpense.Value 
            : null;
    }
}