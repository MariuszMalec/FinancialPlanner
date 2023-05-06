using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;

namespace FinancialPlanner.ConsoleApp.Service
{
    public static class AddUser
    {
        public static void Do(IUserService userService,
            SelectTask currentItem,
            int MinNameLength,
            int MaxNameLength, int MinAge,
            int MaxAge)
        {
            Console.WriteLine($"{currentItem} ...");
            var users = userService.GetAll().Result.ToList();
            if (users.Count > 0)
            {
                var newUser = CreateService.Insert(MinNameLength, MaxNameLength, MinAge, MaxAge);
                if (newUser == null)
                {
                    Console.WriteLine("Exit ...");
                    Environment.Exit(0);
                }
                var check = userService.Insert(newUser).Result;
                if (check == false)
                {
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("User was not created successfully! Press any key to continue.");
                }
                else
                {
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("User created successfully. Press any key to continue.");
                }
                Console.ReadKey();
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
