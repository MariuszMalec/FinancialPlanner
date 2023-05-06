using FinancialPlanner.Logic.Dtos;

namespace FinancialPlanner.ConsoleApp.Service
{
    public class MonthlyBalanceViewer
    {
        public static void Show(List<MonthlyIncomeAndExpenses> balance)
        {
            if (balance.Count() > 0)
            {
                balance.ForEach(x => Console.WriteLine($"{x.Month.ToString("MMMM")} {x.Income} {x.Expenses} {x.Income - x.Expenses} {x.Differrence}"));
            }
        }
    }
}
