using AutoMapper;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class CategoryBudgetController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;

        public CategoryBudgetController(IUserService userService, IMapper mapper, ITransactionService transactionService)
        {
            _userService = userService;
            _mapper = mapper;
            _transactionService = transactionService;
        }

        public async Task<ActionResult> Index(string id, DateTime date)
        {
            var transactions = await _transactionService.GetAllQueryable();

            var userTransactions = transactions.Where(u => u.User.Id == id).ToList();

            var sums = userTransactions.GroupBy(x => x.Category)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Amount).Sum());

            var transactionsDal = userTransactions.Where(x => x.User.Id == id).OrderByDescending(x => x.Date).ToList();

            var userBudget = new List<CategoryBudgetDto>() { new CategoryBudgetDto() { Id = 1,
               UserId =id,
               Date = DateTime.Now,
               Category = Logic.Enums.CategoryOfTransaction.Car,
                PlanedBudget = 0}}; 

            var userBudgetDto = new UsersBudgetDto() { UserId = id,
                Date = date,
                CategorySums = sums,
                UserBudgets = userBudget};

            return View(userBudgetDto);
        }
    }
}
