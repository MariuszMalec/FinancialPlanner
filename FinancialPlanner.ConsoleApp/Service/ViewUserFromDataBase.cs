using FinancialPlanner.ConsoleApp.Validators;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;

namespace FinancialPlanner.ConsoleApp.Service
{
    public static class ViewUserFromDataBase
    {
        public static void Show(IUserService userService, SelectTask currentItem, int MinNameLength, int MaxNameLength)
        {
            Console.WriteLine($"{currentItem} ...(f42969be-3890-423c-8100-4a936abbfb62)");
            var users = userService.GetAll().Result.ToList();
            if (users.Count > 0)
            {
                var id = ValidateUser.GetNonDigString("Id", MinNameLength, MaxNameLength);
                if (id.ToLower() == "exit")
                {
                    Console.WriteLine("Exit ...");
                    Environment.Exit(0);
                }
                var user = userService.GetById(id).Result;
                List<User> showUser = new List<User>() { user };
                if (showUser.Count() > 0 && user != null)
                {
                    UserViewer.Show(showUser);
                    Console.WriteLine($"The user {id} were loaded successful");
                }
                else
                {
                    Console.WriteLine($"The user with id {id} doesn't exist, have not been loaded!");
                }
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
