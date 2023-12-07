using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Globalization;
using ILogger = Serilog.ILogger;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class TransactionUserController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public TransactionUserController(ApplicationDbContext context, IMapper mapper = null, ITransactionService transactionService = null, IUserService userService = null, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _transactionService = transactionService;
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()//TODO tranzakcje z userem
        {
            var transactions = await _transactionService.GetAllQueryable();
            if (transactions == null)
            {
                _logger.Error("Transactions not found at {registrationDate}", DateTime.Now);
                return NotFound("Transactions not found!");
            }
            var model = _mapper.Map<List<TransactionUserDto>>(transactions);

            //current mounth
            var currentMounth = DateTime.Now.Month;
            var userTransactionsByMounth = _transactionService.FilterTransactionByMounth(transactions, currentMounth);
            var incomes = userTransactionsByMounth.Where(x => x.Type == Logic.Enums.TypeOfTransaction.Income).Sum(x => x.Amount);
            var outcomes = userTransactionsByMounth.Where(x => x.Type == Logic.Enums.TypeOfTransaction.Outcome).Sum(x => x.Amount);
            ViewData["MontlyBalance"] = incomes - outcomes;

            _logger.Information("Load user transactions successfully at {registrationDate}", DateTime.Now);
            return _context.Transactions != null ?
                          View(model) :
                          Problem("Entity set 'ApplicationDbContext.Transactions'  is null.");
        }

        public async Task<IActionResult> Select(DateTime? selectMounth)//TODO tranzakcje z userem wg miesiaca
        {

            var transactions = await _transactionService.GetAllQueryable();

            var model = _mapper.Map<List<TransactionUserDto>>(transactions);

            model = model.Where(t=>t.CreatedAt.Month == selectMounth.Value.Month).ToList();

            //current mounth
            var incomes = model.Where(x => x.Type == TypeOfTransaction.Income).Sum(x => x.Amount);
            var outcomes = model.Where(x => x.Type == TypeOfTransaction.Outcome).Sum(x => x.Amount);
            ViewData["MontlyBalance"] = incomes - outcomes;
            var CultureName = "pl-PL";
            ViewData["Income"] = incomes;
            ViewData["Outcome"] = outcomes;
            ViewData["Balance"] = incomes - outcomes;
            ViewData["selectMounthAsInt"] = selectMounth.Value.Month.ToString() ;
            ViewData["CurrentMonth"] = selectMounth.Value.ToString("MMMM", CultureInfo.CreateSpecificCulture(CultureName));
            ViewData["CurrentMonthAsDataTime"] = selectMounth;
            _logger.Information("Load user transactions by selected month successfully at {registrationDate}", DateTime.Now);
            return _context.Transactions != null ?
                          View(model) :
                          Problem("Entity set 'ApplicationDbContext.Transactions'  is null.");
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Transactions == null)
            {
                _logger.Error("Brak transaction {id} at {registrationDate}", id, DateTime.Now);
                return NotFound();
            }

            var transaction = await _transactionService.GetById(id);
            if (transaction == null)
            {
                _logger.Error("Brak transaction {id} at {registrationDate}", id,DateTime.Now);
                return NotFound($"Brak transaction {id}");
            }
            _logger.Information("Load transaction detail successfully at {registrationDate}", DateTime.Now);
            return View(transaction);
        }

        // GET: UserController/Create
        public ActionResult Create(string id)
        {
            ViewData["UserId"] = id;

            var transactions = _transactionService.GetAllQueryable().Result.Where(x => x.UserId == id);
            var currentMounth = DateTime.Now.Month;
            var userTransactionsByMounth = _transactionService.FilterTransactionByMounth(transactions, currentMounth);

            var incomes = userTransactionsByMounth.Where(x => x.Type == TypeOfTransaction.Income).Sum(x => x.Amount);
            var outcomes = userTransactionsByMounth.Where(x => x.Type == TypeOfTransaction.Outcome).Sum(x => x.Amount);
            var balance = incomes - outcomes;
            ViewData["MontlyBalance"] = incomes - outcomes;
            
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string id, TransactionUserDto model)
        {
            if (model == null)
                return NotFound("model transaction not found!");

            var userExist = await _context.Users.FindAsync(model.Id);
            if (userExist == null)
            {
                _logger.Error("Transaction was not created at {registrationDate}", DateTime.Now);
                return NotFound($"Transaction was not created!");
            }

            await _transactionService.Insert(id, model);
            _logger.Information("Create transactions successfully at {registrationDate}", DateTime.Now);
            return RedirectToAction("GetUserTransactionsByMounth", "User", new { model.Id, model.UserId });
        }

        public async Task<ActionResult> Edit(string id)
        {
            var model = await _transactionService.GetById(id);
            ViewData["FullName"] = $"{model.FirstName} {model.LastName}";
            ViewData["UserId"] = id;
            if (model == null)
            {
                _logger.Error("Not found tranasction with {id} at {registrationDate}", id, DateTime.Now);
                return NotFound($"Not found transaction with {id}");
            }
            return View(model);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, TransactionUserDto model)
        {
            //TODO dodac to do serwisu
            var getAmountFromDataBase = _context.Transactions.Where(u => u.Id == model.Id).Select(u => u.Amount).FirstOrDefault();
            var newAmount = (getAmountFromDataBase-model.Amount);
            var getBalance = _context.Users.Where(u => u.Id == model.UserId).Select(u => u.Balance).FirstOrDefault();
            var getAmount = model.Type == Logic.Enums.TypeOfTransaction.Income ? (getBalance - newAmount) : (getBalance + newAmount);
            
            var transaction = new Transaction()
            {
                Id= id,
                UserId = model.UserId,
                Currency = model.Currency,
                Type = model.Type,
                Category = model.Category,
                Amount = model.Amount,
                BalanceAfterTransaction = getAmount,
                Description = model.Description,
                CreatedAt = model.CreatedAt
            };
            _context.Transactions.Update(transaction);
            _context.SaveChanges();

            var user = await _userService.GetById(model.UserId);
            user.Balance = transaction.BalanceAfterTransaction;
            await _userService.Update(user);
            _logger.Information("Transaction was edited successful at {registrationDate}", id, DateTime.Now);
            return RedirectToAction("GetUserTransactionsByMounth", "User", new { model.Id,  model.UserId });
        }

        public async Task<ActionResult> Delete(string id)
        {
            var model = await _context.Transactions.FindAsync(id);

            if (model == null)
            {
                _logger.Error("Not found tranasction with {id} at {registrationDate}", id, DateTime.Now);
                return NotFound($"Not found tranasction with {id}");
            }
            return View(model);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, Transaction model)
        {
            var userId = _transactionService.GetById(id).Result.UserId;
            var check = await _transactionService.Delete(id);
            if (!check)
            {
                _logger.Error("transaction not deleted! at {registrationDate}", id, DateTime.Now);
                return BadRequest("transaction not deleted!");
            }
            _logger.Information("Transaction was deleted successful at {registrationDate}", id, DateTime.Now);
            return RedirectToAction("GetUserTransactionsByMounth", "User", new { model.Id, userId });
        }
    }
}
