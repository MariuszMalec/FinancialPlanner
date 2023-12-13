using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Enums;

namespace FinancialPlanner.Logic.Dtos
{
    public class TransactionUserDto : Entity
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Amount { get; set; }
        public decimal Balance { get; set; }

        public decimal BalanceAfterTransaction { get; set; }

        public Currency Currency { get; set; } = Currency.PLN;

        public TypeOfTransaction Type { get; set; }

        public CategoryOfTransaction Category { get; set; }
        public string? Description { get; set; }

        public string? Picture { get; set; }
    }
}
