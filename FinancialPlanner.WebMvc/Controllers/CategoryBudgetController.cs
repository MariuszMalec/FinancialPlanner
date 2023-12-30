using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using Microsoft.AspNetCore.Mvc;
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

            var userTransactions = transactions.Where(t => t.CreatedAt.Month == selectMounth.Value.Month)
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
    }
}
