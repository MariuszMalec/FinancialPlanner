using FinancialPlanner.Logic.Enums;
using System.Collections;

namespace FinancialPlanner.Logic.Dtos
{
    public class UsersTrendingDto
    {
        public IEnumerable Transactions { get; set; }
        public CategoryOfTransaction Category { get; set; }
        public string UserId { get; set; }
    }
}
