using FinancialPlanner.Logic.Services;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.ConsoleApp.Service;
using FinancialPlanner.Logic.Validation;
using FinancialPlanner.Logic.Enums;
using System.ComponentModel.DataAnnotations;

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
        const int MinAge = 13;
        const int MaxAge = 99;
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
                    if (id == null)
                    {
                        Console.WriteLine("Exit ...");
                        Environment.Exit(0);
                    }

                    var newUser = userService.GetById(id).Result;

                    //readline here
                    var firstName = GetNonDigString("FirstName", MinNameLength);
                    if (firstName != null)
                        newUser.FirstName = firstName;

                    var lastName = GetNonDigString("LastName", MinNameLength);

                    if (lastName != null)
                        newUser.LastName = GetNonDigString("LastName", MinNameLength);

                    var email = GetEmail();
                    if (email != null)
                        newUser.Email = GetEmail();

                    newUser.Gender = GetGender();

                    var balance = GetDecimalInput("current balance");
                    if (balance != -1)
                        newUser.Balance = balance;

                    var age = GetIntInput("Age", MinAge, MaxAge);
                    if (age != 0)
                        newUser.Age = age;

                    userService.Update(newUser);

                    Console.WriteLine("=================================================================");
                    Console.WriteLine("User created successfully! Press any key to continue.");
                    Console.ReadKey();


                    //blad za szybko wyswietlam
                    //var showUser = userService.GetAll().Result.ToList();
                    //if (showUser.Count() > 0 && showUser != null)
                    //{
                    //    UserViewer.Show(showUser);
                    //    Console.WriteLine($"The user {id} were loaded successful");
                    //}
                    //else
                    //{
                    //    Console.WriteLine($"The user have not been loaded!");
                    //}
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

            Console.WriteLine($"write exit or press enter if you want leave");
            Console.Write($"{name}: ");
            input = Console.ReadLine()?.Trim();

            if (input.ToLower() == "exit" || input == "")
                return null;

            if (input == null || input.Length < minLength || input.Length > MaxNameLength || input.Any(char.IsWhiteSpace))
                Console.WriteLine($"Invalid data. {name} should have at least {minLength} char long and in correct format Retry!");
            else
                return input;
        }
    }

    public static string GetEmail()
    {
        while (true)
        {
            Console.Write("email: ");
            var input = Console.ReadLine()?.Trim();

            if (input.ToLower() == "exit" || input == "")
                return null;

            var message = UserValidate.ValidateEmail(input);

            if (string.IsNullOrEmpty(message))
                return input;

            Console.WriteLine(message);
        }
    }

    private static Gender GetGender()
    {
        var genderArray = Enum.GetNames(typeof(Gender));

        Console.WriteLine("Choose your gender:");
        for (int i = 0; i < genderArray.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {genderArray[i]}");
        }
        while (true)
        {
            var input = Console.ReadKey();

            Console.WriteLine();
            if (!char.IsDigit(input.KeyChar))
            {
                Console.WriteLine("Wrong value, try again!\n");
                continue;
            }

            var isParsed = int.TryParse(input.KeyChar.ToString(), out var selection);

            if (isParsed && selection <= genderArray.Length)
                return (Gender)selection - 1;

            Console.WriteLine("Wrong selection, try Again!");
        }
    }

    private static decimal GetDecimalInput(string name)
    {
        while (true)
        {
            Console.WriteLine($"write exit or press enter if you want leave");
            Console.Write($"{name}: ");
            var input = Console.ReadLine().Replace(',', '.');
            if (input.ToLower() == "exit" || input == "")
                return -1;
            var isDig = decimal.TryParse(input, out var result);
            if (isDig && result >= 0)
                return result;

            Console.WriteLine($"{name} should be no less than 0");
        }
    }

    private static int GetIntInput(string name, int min, int max)
    {
        while (true)
        {
            Console.Write($"{name}: ");
            var input = Console.ReadLine();
            if (input.ToLower() == "exit" || input == "")
                return 0;
            var isDig = int.TryParse(input, out var result);
            if (isDig && result >= min && result <= max)
                return result;

            Console.WriteLine($"{name} should be between {min} and {max}");
        }
    }
}