using FinancialPlanner.ConsoleApp.Validators;
using FinancialPlanner.Logic.Models;

namespace FinancialPlanner.ConsoleApp.Service
{
    public static class ViewUserFromJsonFile
    {
        public static void Show(List<User> users, int MinNameLength, int MaxNameLength)
        {
            if (users.Count > 0)
            {
                var id = ValidateUser.GetNonDigString("Id", MinNameLength, MaxNameLength);
                if (id.ToLower() == "exit")
                {
                    Console.WriteLine("Exit ...");
                    Environment.Exit(0);
                }
                var user = users.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
                List<User> showUser = new List<User>() { user };
                if (showUser.Count() > 0 && user != null)
                {
                    UserViewer.Show(showUser);
                    Console.WriteLine($"The user {id} were loaded successful");
                }
                else
                {
                    Console.WriteLine($"The user have not been loaded!");
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
