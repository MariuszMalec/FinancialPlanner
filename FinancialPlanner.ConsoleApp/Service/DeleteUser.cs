using FinancialPlanner.ConsoleApp.Validators;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;

namespace FinancialPlanner.ConsoleApp.Service
{
    public static class DeleteUser
    {
        public static void Do(IUserService<User> userService, SelectTask currentItem, int MinNameLength, int MaxNameLength)
        {
            Console.WriteLine($"{currentItem} ...");
            var users = userService.GetAll().Result.ToList();
            if (users.Count > 0)
            {
                users.ForEach(u => Console.WriteLine($"{u.Id}"));
                var id = ValidateUser.GetNonDigString("Id", MinNameLength, MaxNameLength);
                if (id == null)
                {
                    Console.WriteLine("Exit ...");
                    Environment.Exit(0);
                }
                var selectedUser = userService.GetById(id).Result;
                if (selectedUser == null)
                {
                    Console.WriteLine("Exit ...");
                    Environment.Exit(0);
                }
                var check = userService.Delete(selectedUser).Result;
                if (check == false)
                {
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("User was not deleted successfully! Press any key to continue.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("User deleted successfully. Press any key to continue.");
                    Console.ResetColor();
                }
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"The users have not been loaded!");
            }
        }
    }
}