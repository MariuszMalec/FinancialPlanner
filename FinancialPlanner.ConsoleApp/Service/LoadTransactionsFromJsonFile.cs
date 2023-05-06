using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Services;

namespace FinancialPlanner.ConsoleApp.Service
{
    public static class LoadTransactionsFromJsonFile
    {
        public static void Get(SelectTask currentItem)
        {
            Console.WriteLine($"{currentItem} ...");
            var file = @"Source/transactions.json";
            ILoadData<TransactionDto> loadData = new ReadTransactions<TransactionDto>();
            var transactions = loadData.GetAll(file).ToList();
            if (transactions.Count > 0)
            {
                transactions.ForEach(x => Console.WriteLine($"{x.UserId} {x.Amount} {x.Type} {x.BalanceAfterTransaction} {x.Currency} {x.Category} {x.Date}"));
                Console.WriteLine($"The transaction were loaded successful");
            }
            else
            {
                Console.WriteLine($"The transaction have not been loaded!");
            }
            Console.WriteLine($"Press any key to continue");
            Console.ReadKey();
        }
    }
}
