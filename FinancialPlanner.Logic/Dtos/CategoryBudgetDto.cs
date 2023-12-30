using FinancialPlanner.Logic.Enums;

namespace FinancialPlanner.Logic.Dtos
{
    public class CategoryBudgetDto
    {
        public int Id { get; set; }
        public CategoryOfTransaction Category { get; set; }
        public decimal PlanedBudget { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
