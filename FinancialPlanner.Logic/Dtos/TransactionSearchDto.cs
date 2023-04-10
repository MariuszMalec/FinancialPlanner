using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Models;

namespace FinancialPlanner.Logic.Dtos
{
    public class TransactionSearchDto
    {
        public IEnumerable<Transaction> Transactions { get; set; }
        public CategoryOfTransaction Category { get; set; }
        public TypeOfTransaction Type { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string UserId { get; set; }

    }
}
