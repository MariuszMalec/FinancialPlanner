using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Repository;
using FinancialPlanner.Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class ChartController : Controller
    {
        private readonly IRepository<Transaction> _repository;

        public ChartController(IRepository<Transaction> repository = null)
        {
            _repository = repository;
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
    }
}
