using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

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

        public async Task<ActionResult> Index(string id, DateTime date)
        {
            var transactions = await _transactionService.GetAllQueryable();

            var userTransactions = transactions.Where(u => u.User.Id == id).ToList();

            var sums = userTransactions.GroupBy(x => x.Category)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Amount).Sum());

            var transactionsDal = userTransactions.Where(x => x.User.Id == id).OrderByDescending(x => x.Date).ToList();

            var userBudget = _context.CategoryBudgets.Where(b=>b.UserId == id).ToList();

            //mapowanie
            var categoryBudgetDto = new List<CategoryBudgetDto>();
            userBudget.ForEach(x => categoryBudgetDto.Add(new CategoryBudgetDto() { Id= x.Id,
            UserId = x.UserId,
            Category = x.Category,
            CreatedAt=x.CreatedAt,
            PlanedBudget = x.PlanedBudget}));

            var userBudgetDto = new UsersBudgetDto() { Id = id, 
            CreatedAt = date,
            UserId = id,
            UserBudgets = categoryBudgetDto.ToImmutableList(),
            CategorySums = sums};

            return View(userBudgetDto);
        }
    }
}
