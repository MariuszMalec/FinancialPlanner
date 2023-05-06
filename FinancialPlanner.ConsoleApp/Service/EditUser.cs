using FinancialPlanner.ConsoleApp.Validators;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;

namespace FinancialPlanner.ConsoleApp.Service
{
    public static class EditUser
    {
        public static void Do(
            IUserService userService, 
            SelectTask currentItem, 
            int MinNameLength, 
            int MaxNameLength, int MinAge,
            int MaxAge
            )
        {
            Console.WriteLine($"{currentItem} ...");
            var users = userService.GetAll().Result.ToList();
            if (users.Count > 0)
            {
                var id = ValidateUser.GetNonDigString("Id", MinNameLength, MaxNameLength);
                if (id == null)
                {
                    Console.WriteLine("Exit ...");
                    Environment.Exit(0);
                }
                var selectedUser = userService.GetById(id).Result;
                if (selectedUser != null)
                {
                    selectedUser = EditService.EditUser(selectedUser, MinNameLength, MaxNameLength, MinAge, MaxAge);
                    userService.Update(selectedUser);
                    Console.WriteLine("=================================================================");
                    Console.WriteLine("User edited successfully! Press any key to continue.");
                }
                else
                {
                    Console.WriteLine($"The user with id {id} doesn't exist, have not been loaded!");
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