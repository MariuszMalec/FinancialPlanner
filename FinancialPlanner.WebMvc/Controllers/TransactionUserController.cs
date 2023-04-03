using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class TransactionUserController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly TransactionService _transactionService;
        private readonly UserService _userService;

        public TransactionUserController(ApplicationDbContext context, IMapper mapper = null, TransactionService transactionService = null, UserService userService = null)
        {
            _context = context;
            _mapper = mapper;
            _transactionService = transactionService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()//TODO tranzakcje z userem
        {
            var transactions = await _transactionService.GetAllQueryable();

            var model = _mapper.Map<List<TransactionUserDto>>(transactions);

            return _context.Transactions != null ?
                          View(model) :
                          Problem("Entity set 'ApplicationDbContext.Transactions'  is null.");
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string id, TransactionUserDto model)
        {
            if (model == null)
                return NotFound("Transaction not found!");

            var userExist = await _context.Users.FindAsync(model.Id);
            if (userExist == null)
                return NotFound($"User was not created!");

            var getBalance = _context.Users.Where(u => u.Id == model.Id).Select(u => u.Balance).FirstOrDefault();

            var transaction = new Transaction()
            {
                UserId = id,
                Currency = model.Currency,
                Type = model.Type,
                Category = model.Category,
                Amount = model.Amount,
                BalanceAfterTransaction = getBalance + model.Amount,
                Description = model.Description,
                CreatedAt = model.CreatedAt
            };
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            //zapisac balance w user

            //mapowanie na TransactionUserDto

            return RedirectToAction(nameof(Index));
        }
    }
}
