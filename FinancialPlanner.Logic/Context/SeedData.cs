using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Services;

namespace FinancialPlanner.Logic.Context
{
    public static class SeedData
    {

        private static readonly List<User> _users = LoadDataService.ReadUserFile();

        public static List<User> GetAll()
        {
            return _users;
        }

        public static async void Seed(ApplicationDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }
            await context.Users.AddRangeAsync(_users);
            await context.SaveChangesAsync();
        }

    }
}
