using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
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
            var currentMounth = DateTime.Now;
            var userTransactionsByMounth = _transactionService.FilterTransactionByMounth(transactions, currentMounth);
            var incomes = userTransactionsByMounth.Where(x => x.Type == Logic.Enums.TypeOfTransaction.Income).Sum(x => x.Amount);
            var outcomes = userTransactionsByMounth.Where(x => x.Type == Logic.Enums.TypeOfTransaction.Outcome).Sum(x => x.Amount);
            ViewData["MontlyBalance"] = incomes - outcomes;
            var modelMap = _mapper.Map<List<TransactionUserDto>>(userTransactionsByMounth);
            _logger.Information("Load user transactions successfully at {registrationDate}", DateTime.Now);
            return _context.Transactions != null ?
                          View(modelMap) :
                          Problem("Entity set 'ApplicationDbContext.Transactions'  is null.");
        }

        public async Task<IActionResult> SelectByMonth(DateTime? selectMounth, string id)
        {

            var transactions = await _transactionService.GetAllQueryable();

            if (id != null)
            {
                transactions = transactions.Where(x => x.UserId == id);
                ViewData["UserId"] = id;
            }
            else
            {
                id = _context.Users.First().Id;
                ViewData["UserId"] = "";
            }

            var model = _mapper.Map<List<TransactionUserDto>>(transactions);

            var CultureName = "pl-PL";
            if (selectMounth == null)
            {
                model = model.Where(t => t.CreatedAt.Month == DateTime.Now.Month)
                    .Where(u => u.CreatedAt.Year == DateTime.Now.Year)
                    .ToList();
                ViewData["selectMounthAsInt"] = DateTime.Now.Month.ToString();
                ViewData["CurrentMonth"] = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture(CultureName));
                ViewData["CurrentMonthAsDataTime"] = DateTime.Now;
            }
            else
            {
                model = model.Where(t => t.CreatedAt.Month == selectMounth.Value.Month)
                    .Where(u => u.CreatedAt.Year == selectMounth.Value.Year)
                    .ToList();
                ViewData["selectMounthAsInt"] = selectMounth.Value.Month.ToString();
                ViewData["CurrentMonth"] = selectMounth.Value.ToString("MMMM", CultureInfo.CreateSpecificCulture(CultureName));
                ViewData["CurrentMonthAsDataTime"] = selectMounth;
            }

            //current mounth
            var incomes = model.Where(x => x.Type == TypeOfTransaction.Income).Sum(x => x.Amount);
            var outcomes = model.Where(x => x.Type == TypeOfTransaction.Outcome).Sum(x => x.Amount);
            ViewData["MontlyBalance"] = incomes - outcomes;
            ViewData["Income"] = incomes;
            ViewData["Outcome"] = outcomes;
            ViewData["Balance"] = incomes - outcomes;
            _logger.Information("Load user transactions by selected month successfully at {registrationDate}", DateTime.Now);
            return _context.Transactions != null ?
                          View(model) :
                          Problem("Entity set 'ApplicationDbContext.Transactions'  is null.");
        }

        public async Task<IActionResult> Select(DateTime? selectMounth, string id)//TODO tranzakcje z userem wg miesiaca
        {

            var transactions = await _transactionService.GetAllQueryable();    

            if (id != null)
            {
                transactions = transactions.Where(x => x.UserId == id);
                ViewData["UserId"] = id;
            }
            else
            {
                id = _context.Users.First().Id;
                ViewData["UserId"] = id;
            }

            var model = _mapper.Map<List<TransactionUserDto>>(transactions);

            var CultureName = "pl-PL";
            if (selectMounth == null)
            {
                model = model.Where(t => t.CreatedAt.Month == DateTime.Now.Month)
                    .Where(u => u.CreatedAt.Year == DateTime.Now.Year)
                    .ToList();
                ViewData["selectMounthAsInt"] = DateTime.Now.Month.ToString();
                ViewData["CurrentMonth"] = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture(CultureName));
                ViewData["CurrentMonthAsDataTime"] = DateTime.Now;
            }
            else
            {
                model = model.Where(t => t.CreatedAt.Month == selectMounth.Value.Month)
                    .Where(u => u.CreatedAt.Year == selectMounth.Value.Year)
                    .ToList();
                ViewData["selectMounthAsInt"] = selectMounth.Value.Month.ToString();
                ViewData["CurrentMonth"] = selectMounth.Value.ToString("MMMM", CultureInfo.CreateSpecificCulture(CultureName));
                ViewData["CurrentMonthAsDataTime"] = selectMounth;
            }

            //current mounth
            var incomes = model.Where(x => x.Type == TypeOfTransaction.Income).Sum(x => x.Amount);
            var outcomes = model.Where(x => x.Type == TypeOfTransaction.Outcome).Sum(x => x.Amount);
            ViewData["MontlyBalance"] = incomes - outcomes;
            ViewData["Income"] = incomes;
            ViewData["Outcome"] = outcomes;
            ViewData["Balance"] = incomes - outcomes;
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
            var currentMounth = DateTime.Now;
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
            return RedirectToAction("Index", "User", new { model.Id, model.UserId });
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
            await _transactionService.Edit(id, model);
            return RedirectToAction("GetUserTransactionsByMounth", "User", new { model.Id, model.UserId });
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
