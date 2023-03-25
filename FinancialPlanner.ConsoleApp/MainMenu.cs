using FinancialPlanner.Logic.Services;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Models;
public static class MainMenu
{

    //here you can add new main menu item
    private static readonly string[] mainMenuItem = {
                "Load data from external user file",
                "Load data from external transaction file",
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
        public static void ShowMainMenu()
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
                        users.ForEach(x=>Console.WriteLine($"{x.FirstName} {x.LastName}"));
                        Console.WriteLine($"The Users were loaded successful");
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
}