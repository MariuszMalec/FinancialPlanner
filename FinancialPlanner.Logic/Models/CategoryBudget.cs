using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Enums;

namespace FinancialPlanner.Logic.Models
{
    public class CategoryBudget : Entity
    {
        public CategoryOfTransaction Category { get; set; }
        public decimal PlanedBudget { get; set; } = 0;
        public string UserId { get; set; }
    }
}
