using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Services;

namespace FinancialPlanner.Logic.Context
{
    public class SeedDataFromJson
    {
        private static readonly List<User> _users = LoadDataService<User>.ReadUserFile();

        private static readonly List<Transaction> _transactions = LoadDataService<Transaction>.ReadTransacionFile();

        private static readonly List<TransactionPicture> _transactionPictures = LoadDataService<TransactionPicture>.ReadTransacionPictureFile();
        
        private static readonly List<CategoryBudget> _categoryBudgets = LoadDataService<CategoryBudget>.ReadCategoryBudgetFile();

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
        public static async Task SeedTransactionPictures(ApplicationDbContext context)
        {
            if (context.TransactionPictures.Any())
            {
                return;
            }
            await context.TransactionPictures.AddRangeAsync(_transactionPictures);
            await context.SaveChangesAsync();
        }
        public static async Task SeedCategoryBudget(ApplicationDbContext context)
        {
            if (context.CategoryBudgets.Any())
            {
                return;
            }
            await context.CategoryBudgets.AddRangeAsync(_categoryBudgets);
            await context.SaveChangesAsync();
        }

    }
}
