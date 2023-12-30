using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Enums;

namespace FinancialPlanner.Logic.Dtos
{
    public class UsersBudgetDto : Entity
    {
        public IEnumerable<CategoryBudgetDto> UserBudgets { get; set; }
        public string UserId { get; set; }
        public Dictionary<CategoryOfTransaction, decimal> CategorySums { get; set; }
    }
}
