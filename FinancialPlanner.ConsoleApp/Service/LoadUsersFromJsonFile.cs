using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Services;

namespace FinancialPlanner.ConsoleApp.Service
{
    public static class LoadUsersFromJsonFile
    {
        public static void Get(SelectTask currentItem)
        {
            Console.WriteLine($"{currentItem} ...");
            var file = @"Source/users.json";
            ILoadData<User> loadData = new ReadUsers<User>();
            var users = loadData.GetAll(file).ToList();
            if (users.Count > 0)
            {
                UserViewer.Show(users);
                Console.WriteLine($"The Users were loaded successful");
            }
            else
            {
                Console.WriteLine($"The users have not been loaded!");
            }
            Console.WriteLine($"Press any key to continue");
            Console.ReadKey();
        }
    }
}
