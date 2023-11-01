using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Enums;

namespace FinancialPlanner.Logic.Models
{
    public class TransactionPicture : Entity
    {
        public string? Source { get; set; }
        public virtual CategoryOfTransaction Category { get; set; }
    }
}
