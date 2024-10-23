using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinancialPlanner.Logic.Dtos
{
    public class TransactionUserDto : Entity
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Range(0, 99999.99)]
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }

        public decimal BalanceAfterTransaction { get; set; }

        public Currency Currency { get; set; } = Currency.PLN;

        [Display(Name = "Type")]
        public TypeOfTransaction Type { get; set; } = TypeOfTransaction.Outcome;

        [Display(Name = "Category")]
        public CategoryOfTransaction Category { get; set; } = CategoryOfTransaction.Food;

        [StringLength(25)]
        public string? Description { get; set; }

        public string? Picture { get; set; }
    }
}
