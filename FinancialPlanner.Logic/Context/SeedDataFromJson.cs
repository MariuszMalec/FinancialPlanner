using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Services;

namespace FinancialPlanner.Logic.Context
{
    public class SeedDataFromJson
    {
        private static readonly List<User> _users = LoadDataService<User>.ReadUserFile();

        private static readonly List<Transaction> _transactions = LoadDataService<Transaction>.ReadTransacionFile();

        public static async Task SeedUsers(ApplicationDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }
            await context.Users.AddRangeAsync(_users);
            await context.SaveChangesAsync();
        }
        public static async Task SeedTransactions(ApplicationDbContext context)
        {
            if (context.Transactions.Any())
            {
                return;
            }
            await context.Transactions.AddRangeAsync(_transactions);
            await context.SaveChangesAsync();
        }
    }
}
