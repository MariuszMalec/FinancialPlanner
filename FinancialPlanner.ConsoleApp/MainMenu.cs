using FinancialPlanner.ConsoleApp.Service;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Services;

public static class MainMenu
{
    //here you can add new main menu item
    private static readonly SelectTask[] mainMenuItem = {
                SelectTask.Load_users_from_json_file,
                SelectTask.Load_transactions_from_json_file,
                SelectTask.View_user_from_json_file,
                SelectTask.Load_users_from_database,
                SelectTask.View_user,
                SelectTask.YearlyBalance,
                SelectTask.Add_new_user,
                SelectTask.Edit_user,
                SelectTask.Delete_user,
                SelectTask.Exit};

        public const int MinNameLength = 2;
        public const int MaxNameLength = 50;
        public const int MinAge = 13;
        public const int MaxAge = 99;

        public static void ShowMainMenu(IUserService userService, ITransactionService transactionService)
        {
            short currentItem = 0;           
            do
            {
                ConsoleKeyInfo keyPressed;
                do
                {
                    Console.Clear();
                    Console.WriteLine("========================================================");
                    Console.WriteLine("========================================================");
                    Console.WriteLine("||    Welcome in the financial planner by TheBTeam    ||");
                    Console.WriteLine("========================================================");
                    Console.WriteLine("========================================================");
                    for (int i = 0; i < mainMenuItem.Length; i++)
                    {
                        if (currentItem == i)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(">>");
                            Console.WriteLine(mainMenuItem[i] + "<<");
                        }
                        else
                        {
                            Console.WriteLine(mainMenuItem[i]);
                        }
                        Console.ResetColor();
                    }
                    Console.WriteLine("-----------------------------------------------");
                    Console.Write("Select your choice with the arrow keys and click (ENTER) key");
                    keyPressed = Console.ReadKey(true);
                    Console.Clear();
                    if (keyPressed.Key.ToString() == "DownArrow")
                    {
                        currentItem++;
                        if (currentItem > mainMenuItem.Length - 1) currentItem = 0;
                    }
                    else if (keyPressed.Key.ToString() == "UpArrow")
                    {
                        currentItem--;
                        if (currentItem < 0) currentItem = Convert.ToInt16(mainMenuItem.Length - 1);
                    }
                } while (keyPressed.KeyChar != 13);//if press enter selected menu
            if (mainMenuItem[currentItem] == SelectTask.Load_users_from_json_file)//thing it is better way
            {
                LoadUsersFromJsonFile.Get(mainMenuItem[currentItem]);
            }
            else if (mainMenuItem[currentItem] == SelectTask.View_user_from_json_file)//thing it is better way
            {
                Console.WriteLine($"{mainMenuItem[currentItem]} ...");
                var users = LoadDataService<User>.ReadUserFile();
                ViewUserFromJsonFile.Show(users, MinNameLength, MaxNameLength);
            }
            else if (mainMenuItem[currentItem] == SelectTask.Load_transactions_from_json_file)//thing it is better way
            {
                LoadTransactionsFromJsonFile.Get(mainMenuItem[currentItem]);
            }
            else if (mainMenuItem[currentItem] == SelectTask.Load_users_from_database)//thing it is better way
            {
                LoadUsersFromDataBase.Get(userService, mainMenuItem[currentItem]);
            }
            else if (mainMenuItem[currentItem] == SelectTask.View_user)//thing it is better way
            {
                ViewUserFromDataBase.Show(userService, mainMenuItem[currentItem], MinNameLength, MaxNameLength);
            }
            else if (mainMenuItem[currentItem] == SelectTask.YearlyBalance)//thing it is better way
            {
                YearlyBalance.Show(transactionService, mainMenuItem[currentItem]);
            }
            else if (mainMenuItem[currentItem] == SelectTask.Edit_user)//thing it is better way
            {
                EditUser.Do(userService, mainMenuItem[currentItem], MinNameLength, MaxNameLength, MinAge, MaxAge);
            }
            else if (mainMenuItem[currentItem] == SelectTask.Add_new_user)//thing it is better way
            {
                AddUser.Do(userService, mainMenuItem[currentItem], MinNameLength, MaxNameLength, MinAge, MaxAge);

            }
            else if (mainMenuItem[currentItem] == SelectTask.Delete_user)
            {
                DeleteUser.Do(userService, mainMenuItem[currentItem], MinNameLength, MaxNameLength);
                Console.WriteLine($"Press any key to continue");
                Console.ReadKey();
            }
            else if (mainMenuItem[currentItem] == SelectTask.Exit)
            {
                Console.WriteLine("Exit ...");
                Environment.Exit(0);
            }
            else
                {
                    Console.WriteLine("Will be soon!");
                    Console.ReadKey();
                }
            } while (true);
        }
}