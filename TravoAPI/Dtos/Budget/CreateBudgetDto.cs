using System.ComponentModel.DataAnnotations;

namespace TravoAPI.Dtos.Budget
{
    public class CreateBudgetDto
    {
        [Required]
        public DateTime Day { get; set; }

        [Required, MaxLength(100)]
        public string Category { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required]
        public decimal Amount { get; set; }


        [Required]
        [RegularExpression("Pending|Paid")]
        public string Status { get; set; }
    }
}
