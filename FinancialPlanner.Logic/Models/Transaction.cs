using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanner.Logic.Models
{
    public class Transaction : Entity
    {
        public Currency Currency { get; set; }
        public TypeOfTransaction Type { get; set; }

        [Required]
        public User User { get; set; }
        public int UserId { get; set; }
        public CategoryOfTransaction Category { get; set; }

        [Required]
        [Range(0, double.PositiveInfinity)]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+(.\d{1,2})?$")]
        public decimal Amount { get; set; }

        [DataType(DataType.Currency)]
        public decimal BalanceAfterTransaction { get; set; }
        public string Description { get; set; }

        public DateTime Date { get; set; }

    }
}
