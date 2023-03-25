using FinancialPlanner.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanner.ConsoleApp
{
    public class AppStart
    {
        private readonly IUserService _userService;

        public AppStart(IUserService userService)
        {
            _userService = userService;
        }

        public void Run(string[] args)
        {

            MainMenu.ShowMainMenu(_userService);

            //var users = _userService.GetAll().Result;
            //users.ToList().ForEach(x => Console.WriteLine($"{x.FirstName} {x.LastName}"));

            Console.WriteLine($"Application running");
        }
    }
}
