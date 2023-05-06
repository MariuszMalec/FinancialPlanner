using System.ComponentModel.DataAnnotations;

namespace FinancialPlanner.Logic.Dtos
{
    public class MonthlyIncomeAndExpenses
    {
        public DateTime Month { get; set; }
        //[DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]//TODO use it if not added format in program.cs
        [DataType(DataType.Currency)]
        public decimal Income { get; set; }
        [DataType(DataType.Currency)]
        public decimal Expenses { get; set; }
        [DataType(DataType.Currency)]
        public decimal Differrence { get; set; }
        [DataType(DataType.Currency)]
        public decimal IncomeMinusOutCome
        {
            get
            {
                return (Income - Expenses);
            }
            private set
            {
                IncomeMinusOutCome = (Income - Expenses);
            }
        }
    }
}
