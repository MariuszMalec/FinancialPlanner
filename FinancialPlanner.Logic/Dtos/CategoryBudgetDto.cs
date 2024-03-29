﻿using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Enums;

namespace FinancialPlanner.Logic.Dtos
{
    public class CategoryBudgetDto : Entity
    {
        public CategoryOfTransaction Category { get; set; }
        public decimal PlanedBudget { get; set; }
        public string UserId { get; set; }
    }
}
