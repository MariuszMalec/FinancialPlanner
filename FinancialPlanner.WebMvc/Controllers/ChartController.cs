using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Repository;
using FinancialPlanner.Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class ChartController : Controller
    {
        private readonly IRepository<Transaction> _repository;
        private readonly ITransactionService _transactionService;

        public ChartController(IRepository<Transaction> repository = null, ITransactionService transactionService = null)
        {
            _repository = repository;
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Index(string id)
        {
            ViewBag.UserId = String.IsNullOrEmpty(id) ? "UserId" : "";

            var transactions = _repository.GetAll().Result.Where(u=>u.UserId == id).ToList();

            if (transactions.Count() == 0)
            {
                return BadRequest("No transactions!");
            }

            var sums = transactions.GroupBy(x => x.Category.ToString())
                .ToDictionary(x => x.Key, x => x.Select(y => y.Amount).Sum());

            return View(new ActivityStatisticsView() { ActivitySums = sums });
        }

        public async Task<IActionResult> GetChart(string id)
        {
            ViewBag.UserId = String.IsNullOrEmpty(id) ? "UserId" : "";


            //current mounth
            var currentMounth = DateTime.Now;
            var transactions = _repository.GetAll().Result.Where(u => u.UserId == id).AsQueryable();
            var userTransactionsByMounth = _transactionService.FilterTransactionByMounth(transactions, currentMounth);

            if (userTransactionsByMounth.Count() == 0)
            {
                return BadRequest("No transactions!");
            }

            //wydatki
            var sumOutcome = userTransactionsByMounth.Where(t => t.Type == Logic.Enums.TypeOfTransaction.Outcome)
                                   .Select(t => t.Amount).Sum();
            //wplywy
            var sumIncome = userTransactionsByMounth.Where(t => t.Type == Logic.Enums.TypeOfTransaction.Income)
                                   .Select(t => t.Amount).Sum();

            var transactionWithUser = _repository.GetAllQueryable()
                                                .Include(u => u.User);
            //srodki
            var balance = transactionWithUser
                .Where(t=>t.UserId == id)
                .Where(t => t.User.Balance > 0).Select(t => t.User.Balance)
                .FirstOrDefault();
            ViewData["Balance"] = balance;
            ViewData["Income"] = sumIncome;
            ViewData["Outcome"] = sumOutcome;

            //TODO tu skonczylem wykres procentowy chcem pokazac, zamienic na double

            //TODO dodanie do wykresu pozostale aby pokazac ogolny procent
            userTransactionsByMounth.ToList().Add(new Transaction ()
            {
                UserId = id,
                Amount = balance,
                BalanceAfterTransaction = balance,
                Type = Logic.Enums.TypeOfTransaction.Outcome,
                Category = Logic.Enums.CategoryOfTransaction.Other,
                Date= DateTime.Now,
                Description="Another",
                CreatedAt= DateTime.Now,
                Currency=Logic.Enums.Currency.PLN,
                User = transactionWithUser.Where(u=>u.UserId == id).Select(u=>u.User).FirstOrDefault()
            });

            if (balance == 0 )
            {
                return BadRequest("budzet zostal przekroczony!");
            }

            var sums = new Dictionary<string, decimal>();
            if (sumIncome > 0)
            {
                sums = userTransactionsByMounth.Where(x => x.Type == Logic.Enums.TypeOfTransaction.Outcome)
                    .GroupBy(x => x.Category.ToString())
                    .ToDictionary(x => x.Key, x => x.Select(y => ((y.Amount))).Sum());
            }
            else
            {
                sums = userTransactionsByMounth.Where(x => x.Type == Logic.Enums.TypeOfTransaction.Outcome)
                    .GroupBy(x => x.Category.ToString())
                    .ToDictionary(x => x.Key, x => x.Select(y => ((y.Amount))).Sum());
            }

            return View(new IncomeOutcomeDto() { IncomeOutcomeSum = sums });
        }
    }
}
