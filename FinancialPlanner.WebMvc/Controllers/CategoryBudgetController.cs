using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Immutable;
using System.Globalization;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class CategoryBudgetController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly ApplicationDbContext _context;

        public CategoryBudgetController(IUserService userService, IMapper mapper, ITransactionService transactionService, ApplicationDbContext context)
        {
            _userService = userService;
            _mapper = mapper;
            _transactionService = transactionService;
            _context = context;
        }

        public async Task<ActionResult> Index(DateTime? selectMounth, string id)
        {
            var transactions = await _transactionService.GetAllQueryable();

            if (selectMounth == null)
            {
                selectMounth = DateTime.Now;
            }

            var userTransactions = transactions.Where(t => t.CreatedAt.Month == selectMounth.Value.Month)
                .Where(u => u.CreatedAt.Year == selectMounth.Value.Year)
                .Where(u => u.User.Id == id).ToList();

            var sums = userTransactions.GroupBy(x => x.Category)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Amount).Sum());

            var userBudget = _context.CategoryBudgets.Where(b=>b.UserId == id).ToList();

            //mapowanie
            var categoryBudgetDto = new List<CategoryBudgetDto>();
            userBudget.ForEach(x => categoryBudgetDto.Add(new CategoryBudgetDto() { Id= x.Id,
            UserId = x.UserId,
            Category = x.Category,
            CreatedAt=x.CreatedAt,
            PlanedBudget = x.PlanedBudget}));

            var userBudgetDto = new UsersBudgetDto() { Id = id, 
            CreatedAt = selectMounth.Value,
            UserId = id,
            UserBudgets = categoryBudgetDto.ToImmutableList(),
            CategorySums = sums};

            if (!userBudget.Any())
            {
                return Content("No budget!");
            }

            var CultureName = "pl-PL";
            ViewData["selectMounthAsInt"] = selectMounth.Value.Month.ToString();
            ViewData["CurrentMonth"] = selectMounth.Value.ToString("MMMM", CultureInfo.CreateSpecificCulture(CultureName));
            ViewData["CurrentMonthAsDataTime"] = selectMounth;

            var incomes = userTransactions.Where(x => x.Type == TypeOfTransaction.Income).Sum(x => x.Amount);
            var outcomes = userTransactions.Where(x => x.Type == TypeOfTransaction.Outcome).Sum(x => x.Amount);
            ViewData["MontlyBalance"] = incomes - outcomes;
            ViewData["Income"] = incomes;
            ViewData["Outcome"] = outcomes;
            ViewData["Balance"] = incomes - outcomes;

            return View(userBudgetDto);
        }

        public ActionResult UsersTrending(string id, CategoryOfTransaction category, DateTime dateFrom, DateTime dateTo)
        {
            CheckDateToTrending(ref dateFrom, ref dateTo);

            var groupedTransactions = GroupTransactionForTrending(id, category, dateFrom, dateTo);

            return View(new UsersTrendingDto() { Transactions = groupedTransactions, Category = category, UserId = id });
        }

        private void CheckDateToTrending(ref DateTime dateFrom, ref DateTime dateTo)
        {
            var now = DateTime.UtcNow;
            var currentMonth = new DateTime(now.Year, now.Month, 1);
            var pastMonth = currentMonth.AddMonths(-2).AddDays(-1);
            dateFrom = dateFrom == default ? new DateTime(now.Year, now.Month, 1).AddMonths(-3) : dateFrom;
            dateTo = dateTo == default ? DateTime.UtcNow : dateTo;
        }

        private IEnumerable GroupTransactionForTrending(string id, CategoryOfTransaction category, DateTime dateFrom, DateTime dateTo)
        {
            var transactions = _transactionService.GetAllQueryable().Result;

            var userTransactions = transactions.Where(x => x.User.Id == id)
                .Where(x => x.CreatedAt > dateFrom && x.CreatedAt < dateTo).Where(x => x.Category == category).ToList();

            var groupedTransactions = userTransactions.Select(x => new { x.CreatedAt.Year, x.CreatedAt.Month, x.Amount })
                .GroupBy(y => new { y.Year, y.Month },
                    (key, group) => new { year = key.Year, month = key.Month, sum = @group.Sum(x => x.Amount) })
                .OrderBy(d => d.year).ThenBy(d => d.month)
                .GroupBy(x => new { x.year, x.month, x.sum }, (key, group) => new { date = $"{key.month}.{key.year}", sum = key.sum })
                .ToList();
            return groupedTransactions;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBudget(string id, IFormCollection collection)
        {
            foreach (var category in collection)
            {
                //TODO: obsłużyć brakujace
                if (Enum.TryParse<CategoryOfTransaction>(category.Key, out var categoryEnum))
                {
                    var existing = _context.CategoryBudgets
                        .FirstOrDefault(x => x.UserId == id && x.Category == categoryEnum) ?? new CategoryBudget() { UserId = id, Category = categoryEnum };

                    var stringValue = category.Value.FirstOrDefault();
                    existing.PlanedBudget = string.IsNullOrWhiteSpace(stringValue) ? 0M : decimal.Parse(stringValue);

                    _context.Update(existing);
                }
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { id });
        }
    }
}
