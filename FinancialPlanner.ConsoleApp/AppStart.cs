using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Services;
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
        private readonly IUserService<User> _userService;
        private readonly ITransactionService _transactionService;

        public AppStart(IUserService<User> userService, ITransactionService transactionService)
        {
            _userService = userService;
            _transactionService = transactionService;
        }

        public void Run(string[] args)
        {

            MainMenu.ShowMainMenu(_userService, _transactionService);

            //var users = _userService.GetAll().Result;
            //users.ToList().ForEach(x => Console.WriteLine($"{x.FirstName} {x.LastName}"));

            Console.WriteLine($"Application running");
        }
    }
}
