using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravoAPI.Models.Enums;

namespace TravoAPI.Models
{
    public class Budget
    {
        public int Id { get; set; }
        
        [Required]
        public DateTime Day { get; set; }

        [Required, MaxLength(100)]
        public string Category { get; set; }
        
        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public BudgetStatus Status { get; set; }
        
        [Required]
        public string UserId { get; set; }

        [Required]
        public int TripId { get; set; }

        public ApplicationUser User { get; set; }


    }
}
