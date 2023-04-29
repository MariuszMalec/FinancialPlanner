using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanner.ConsoleApp.Service
{
    public class MonthlyBalanceViewer
    {
        public static void Show(List<MonthlyIncomeAndExpenses> balance)
        {
            if (balance.Count() > 0)
            {
                balance.ForEach(x => Console.WriteLine($"{x.Month} {x.Income} {x.Expenses}"));
            }
        }
    }
}
