using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;

namespace FinancialPlanner.ConsoleApp.Service
{
    public static class YearlyBalance
    {
        public static void Show(ITransactionService transactionService, SelectTask currentItem)
        {
            Console.WriteLine($"{currentItem} ...");
            var transactions = transactionService.GetAllQueryable().Result;
            var mouthlyBalance = transactionService.FilterByYearBalance(transactions).Result;//0-styczen
            if (mouthlyBalance.Count() > 0)
            {
                MonthlyBalanceViewer.Show(mouthlyBalance.ToList());
                Console.WriteLine($"The transactions were loaded successful");
            }
            else
            {
                Console.WriteLine($"The transactions have not been loaded!");
            }
            Console.WriteLine($"Press any key to continue");
            Console.ReadKey();
        }
    }
}
