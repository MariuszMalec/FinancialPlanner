using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class TransactionUserPageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;

        public TransactionUserPageController(ApplicationDbContext context, IMapper mapper, ITransactionService transactionService, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _transactionService = transactionService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(int? page, DateTime? selectMounth)
        {
            var transactions = await _transactionService.GetAllQueryable();

            var model = _mapper.Map<List<TransactionUserDto>>(transactions);

            model = model.OrderByDescending(t=>t.CreatedAt).ToList();

            var pageNumber = page ?? 1;
            var perPage = 5;
            var onePageOfTransaction = await model.ToPagedListAsync(pageNumber, perPage);


            return onePageOfTransaction != null ?
                          View(onePageOfTransaction) :
                          NotFound("transaction is null!");
        }
    }
}
