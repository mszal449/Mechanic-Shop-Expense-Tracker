using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsiteApi.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }

        [ForeignKey("Job")]
        public int JobId { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        // Navigation property
        public Job Job { get; set; }
    }
}
