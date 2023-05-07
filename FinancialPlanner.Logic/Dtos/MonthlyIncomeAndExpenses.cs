using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialPlanner.Logic.Dtos
{
    public class MonthlyIncomeAndExpenses
    {
        public DateTime Month { get; set; }
        //[DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]//TODO use it if not added format in program.cs
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Income { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Expenses { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
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
