using FinancialPlanner.Logic.Services;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.ConsoleApp.Service;

public static class MainMenu
{

    //here you can add new main menu item
    private static readonly string[] mainMenuItem = {
                "Load data from external user file",
                "View users from file",
                "Load data from external transaction file",
                "Load data from external user from database",
                "View user",
                "Edit user",
                "Add new user",
                "View users",
                "Enter transaction",
                "Show all transaction",
                "Show transaction by Category",
                "Show transaction according User",
                "Show Users transaction by Type",
                "Edit existing user",
                "Edit transaction",
                "Exit" };

        const int MinNameLength = 2;
        const int MaxNameLength = 50;
        public static void ShowMainMenu(IUserService userService)
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
                if (mainMenuItem[currentItem] == "Load data from external user file")//thing it is better way
                {
                    Console.WriteLine($"{mainMenuItem[currentItem]} ...");
                    var users = LoadDataService<User>.ReadUserFile();
                    if (users.Count > 0)
                    {
                      //users.ForEach(x=>Console.WriteLine($"{x.FirstName} {x.LastName}"));
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
            else if (mainMenuItem[currentItem] == "View user from file")//thing it is better way
            {
                Console.WriteLine($"{mainMenuItem[currentItem]} ...");

                var users = LoadDataService<User>.ReadUserFile();

                if (users.Count > 0)
                {
                    //users.ForEach(x => Console.WriteLine($"{x.FirstName} {x.LastName}"));

                    var id = GetNonDigString("Id", MinNameLength);
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
            else if (mainMenuItem[currentItem] == "Load data from external transaction file")//thing it is better way
                {
                    Console.WriteLine($"{mainMenuItem[currentItem]} ...");
                    var transactions = LoadDataService<TransactionDto>.ReadTransacionFile().ToList();
                    if (transactions.Count > 0)
                    {
                        transactions.ForEach(x=>Console.WriteLine($"{x.UserId} {x.Amount} {x.Type} {x.BalanceAfterTransaction} {x.Currency} {x.Category} {x.Date}"));
                        Console.WriteLine($"The transaction were loaded successful");
                    }
                    else
                    {
                        Console.WriteLine($"The transaction have not been loaded!");
                    }
                    Console.WriteLine($"Press any key to continue");
                    Console.ReadKey();
                }
                else if (mainMenuItem[currentItem] == "Load data from external user from database")//thing it is better way
                {
                    Console.WriteLine($"{mainMenuItem[currentItem]} ...");

                    var users = userService.GetAll().Result.ToList();

                    if (users.Count > 0)
                    {
                    //users.ForEach(x => Console.WriteLine($"{x.FirstName} {x.LastName}"));
                    UserViewer.Show(users);
                    Console.WriteLine($"The users were loaded successful");
                    }
                    else
                    {
                        Console.WriteLine($"The users have not been loaded!");
                    }
                    Console.WriteLine($"Press any key to continue");
                    Console.ReadKey();
                }
                else if (mainMenuItem[currentItem] == "View user")//thing it is better way
                {
                    Console.WriteLine($"{mainMenuItem[currentItem]} ...");

                    var users =userService.GetAll().Result.ToList();

                    if (users.Count > 0)
                    {
                    //users.ForEach(x => Console.WriteLine($"{x.FirstName} {x.LastName}"));

                    var id = GetNonDigString("Id", MinNameLength);
                    if (id.ToLower() == "exit")
                    {
                        Console.WriteLine("Exit ...");
                        Environment.Exit(0);
                    }
                    var user = userService.GetById(id).Result;
                    List<User> showUser = new List<User>() { user};
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
            else if (mainMenuItem[currentItem] == "Edit user")//thing it is better way
            {
                Console.WriteLine($"{mainMenuItem[currentItem]} ...");

                var users = userService.GetAll().Result.ToList();

                if (users.Count > 0)
                {
                    //users.ForEach(x => Console.WriteLine($"{x.FirstName} {x.LastName}"));

                    var id = GetNonDigString("Id", MinNameLength);
                    if (id.ToLower() == "exit")
                    {
                        Console.WriteLine("Exit ...");
                        Environment.Exit(0);
                    }

                    var newUser = userService.GetById(id).Result;

                    //readline here
                    newUser.FirstName = "test1";

                    userService.Update(newUser);

                    //blad za szybko wyswietlam
                    var showUser = userService.GetAll().Result.ToList();
                    if (showUser.Count() > 0 && showUser != null)
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
            else if (mainMenuItem[currentItem] == ("Exit"))
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


    public static string GetNonDigString(string name, int minLength)
    {
        while (true)
        {
            var input = string.Empty;
            if (minLength == 0)
            {
                Console.Write($"{name}(press enter if null): ");
                return Console.ReadLine()?.Trim();
            }

            Console.WriteLine($"Press exit if want leave");
            Console.Write($"{name}: ");
            input = Console.ReadLine()?.Trim();
            if (input == null || input.Length < minLength || input.Length > MaxNameLength || input.Any(char.IsWhiteSpace))
                Console.WriteLine($"Invalid data. {name} should have at least {minLength} char long and in correct format Retry!");
            else
                return input;
        }
    }
}