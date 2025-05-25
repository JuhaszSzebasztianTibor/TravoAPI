using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TravoAPI.Dtos.Budget
{
    public class BudgetDto
    {
        [BindNever]
        public int Id { get; set; }

        [BindNever]
        public int TripId { get; set; }

        [BindNever]
        public string UserId { get; set; }

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
