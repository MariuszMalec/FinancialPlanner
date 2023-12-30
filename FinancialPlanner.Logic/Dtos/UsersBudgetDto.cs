using FinancialPlanner.Logic.Enums;

namespace FinancialPlanner.Logic.Dtos
{
    public class UsersBudgetDto
    {
        public IEnumerable<CategoryBudgetDto> UserBudgets { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<CategoryOfTransaction, decimal> CategorySums { get; set; }
    }
}
