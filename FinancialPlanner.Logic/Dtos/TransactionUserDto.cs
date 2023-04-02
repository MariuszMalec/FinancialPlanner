using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanner.Logic.Dtos
{
    public class TransactionUserDto : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Balance { get; set; }

        public decimal BalanceAfterTransaction { get; set; }

        public Currency Currency { get; set; }

        public TypeOfTransaction Type { get; set; }

        public CategoryOfTransaction Category { get; set; }
        public string? Description { get; set; }
    }
}
