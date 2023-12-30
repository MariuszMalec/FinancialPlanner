using AutoMapper;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;

        private ILogger _logger;

        public UserController(IUserService userService, IMapper mapper = null, ITransactionService transactionService = null, ILogger logger = null)
        {
            _userService = userService;
            _mapper = mapper;
            _transactionService = transactionService;
            _logger = logger;
        }

        // GET: UserController

        public async Task<IActionResult> GetUserTransactions(string id, string userId, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Amount" : "";
            ViewBag.DateSortParm = sortOrder == "CreatedAt" ? "date_desc" : "CreatedAt";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "type_desc" : "Type";

            //TODO jak zrobic inaczej przekazuje raz id tranazakcji a raz id usera!
            if (userId == null)
            {
                userId = id;
            }

            ViewData["UserId"] = id;

            var currentUser = await _userService.GetById(userId);//TODO to w tescie null

            if (currentUser != null)
                ViewData["FullName"] = $"{currentUser.FirstName} {currentUser.LastName}";

            var transactions = await _transactionService.GetAllQueryable();

            var userTransactions = transactions.Where(u=>u.User.Id == userId).ToList();

            var sorted = from s in userTransactions
                         select s;
            switch (sortOrder)
            {
                case "Amount":
                    sorted = sorted.OrderByDescending(s => s.Amount);
                    break;
                case "CreatedAt":
                    sorted = sorted.OrderByDescending(s => s.CreatedAt);
                    break;
                case "date_desc":
                    sorted = sorted.OrderBy(s => s.CreatedAt);
                    break;
                case "Type":
                    sorted = sorted.OrderByDescending(s => s.Type);
                    break;
                case "type_desc":
                    sorted = sorted.OrderBy(s => s.Type);
                    break;
                default:
                    sorted = sorted.OrderBy(s => s.Amount);
                    break;
            }

            var model = _mapper.Map<List<TransactionUserDto>>(sorted);

            var incomes = model.Where(x => x.Type == Logic.Enums.TypeOfTransaction.Income).Sum(x => x.Amount);
            var outcomes = model.Where(x => x.Type == Logic.Enums.TypeOfTransaction.Outcome).Sum(x => x.Amount);
            ViewData["MontlyBalance"] = incomes - outcomes;
            _logger.Information("Load user transactions successfully at {registrationDate}", DateTime.Now);
            return model != null ?
                          View(model) :
                          Problem("Entity set 'ApplicationDbContext.Transactions'  is null.");
        }

        public async Task<IActionResult> GetUserTransactionsByMounth(string id, string userId, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Amount" : "";
            ViewBag.DateSortParm = sortOrder == "CreatedAt" ? "date_desc" : "CreatedAt";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "type_desc" : "Type";

            //TODO jak zrobic inaczej przekazuje raz id tranazakcji a raz id usera!
            if (userId == null)
            {
                userId = id;
            }

            ViewData["UserId"] = id;

            var currentUser = await _userService.GetById(userId);

            if (currentUser != null)
                ViewData["FullName"] = $"{currentUser.FirstName} {currentUser.LastName}";

            var transactions = await _transactionService.GetAllQueryable();

            //current mounth
            var currentMounth = DateTime.Now.Month;
            var userTransactionsByMounth = _transactionService.FilterTransactionByMounth(transactions, currentMounth);

            var userTransactions = userTransactionsByMounth.Where(u => u.User.Id == userId).ToList();

            var sorted = from s in userTransactions
                         select s;
            switch (sortOrder)
            {
                case "Amount":
                    sorted = sorted.OrderByDescending(s => s.Amount);
                    break;
                case "CreatedAt":
                    sorted = sorted.OrderByDescending(s => s.CreatedAt);
                    break;
                case "date_desc":
                    sorted = sorted.OrderBy(s => s.CreatedAt);
                    break;
                case "Type":
                    sorted = sorted.OrderByDescending(s => s.Type);
                    break;
                case "type_desc":
                    sorted = sorted.OrderBy(s => s.Type);
                    break;
                default:
                    sorted = sorted.OrderBy(s => s.Amount);
                    break;
            }

            var model = _mapper.Map<List<TransactionUserDto>>(sorted);

            var incomes = model.Where(x=>x.Type == Logic.Enums.TypeOfTransaction.Income).Sum(x => x.Amount);
            var outcomes = model.Where(x => x.Type == Logic.Enums.TypeOfTransaction.Outcome).Sum(x => x.Amount);
            ViewData["MontlyBalance"] = incomes - outcomes;
            _logger.Information("Load user transactions by month successfully at {registrationDate}", DateTime.Now);
            return model != null ?
                          View(model) :
                          Problem("Entity set 'ApplicationDbContext.Transactions'  is null.");
        }

        public async Task<IActionResult> GetMonthlyIncomeAndExpenses(string id, string userId)
        {
            if (userId == null)
            {
                userId = id;
            }
            ViewData["UserId"] = id;

            var currentUser = await _userService.GetById(userId);

            ViewData["FullName"] = $"{currentUser.FirstName} {currentUser.LastName}";

            var transactions = await _transactionService.GetAllQueryable();

            var userTransactions = transactions.Where(u => u.User.Id == userId);

            var model = _transactionService.FilterByYearBalance(userTransactions).Result;  

            if (model == null)
            {
                return NotFound();
            }
            if (model.Count() == 0)
            {
                _logger.Error("Transactions not found!");
                return NotFound("Transactions not found!");
            }
            _logger.Information("Load user expanses by month successfully at {registrationDate}", DateTime.Now);
            return View(model);
        }

        public async Task<ActionResult<List<UserDto>>> Index()
        {
            var users = await _userService.GetAllQueryable();//TODO dodalem do usera role!!

            if (!users.Any())
            {
                return View("No users!");
            }

            //TODO automapper chyba gorszy bo trzeba szukac profile
            var model = _mapper.Map<List<UserDto>>(users).OrderBy(x => x.CreatedAt);

            //TODO standard mapping
            //var model = users.Select(x=> new UserDto() 
            //{ 
            //    Id = x.Id , 
            //    CreatedAt = x.CreatedAt, 
            //    Company = x.Company,
            //    FirstName = x.FirstName,
            //    LastName = x.LastName,
            //    Email = x.Email,
            //    IsActive = x.IsActive
            //});
            _logger.Information("Load users successfully at {registrationDate}", DateTime.Now);
            return View(model);
        }

        // GET: UserController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var user = await _userService.GetById(id);

            if (user == null)
            {
                _logger.Error($"Brak uzytkownika {id}");
                return BadRequest($"Brak uzytkownika {id}");
            }
            _logger.Information("Load details of user successfully at {registrationDate}", DateTime.Now);
            return View(user);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserDto model)
        {
            if (model == null)
            {
                _logger.Error($"404 bledny model!");
                return NotFound("404 bledny model!");
            }
            //mapowanie na user
            var newUser = new User()
            {
                Company =model.Company,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsActive = model.IsActive,
                Age=99,
                Balance=model.Balance,
                Currency = Logic.Enums.Currency.PLN
            };
            var check = await _userService.Insert(newUser);
            if (check == false)
            {
                _logger.Error($"User was not created!");
                return NotFound($"User was not created!");
            }
            _logger.Information($"Dodano uzytkownika {newUser.LastName}");
            return RedirectToAction(nameof(Index));
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var model = await _userService.GetById(id);
            if (model == null)
            {
                _logger.Error($"Not found user with {id}!");
                return NotFound($"Not found user with {id}");
            }
            return View(model);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, User model)
        {
                await _userService.Update(model);
                if (model.Id == null)
                {
                    return NotFound("No user!");
                }
                _logger.Information($"Edytowano uzytkownika {model.Id}");
                return RedirectToAction(nameof(Index));
        }

        // GET: UserController/Delete/5
        public async Task<ActionResult<User>> Delete(string id)
        {
            var model = await _userService.GetById(id);
            if (model == null)
            {
                _logger.Error($"Not found user with {id}!");
                return NotFound($"Not found user with {id}");
            }
            return View(model);
        }

        // POST: RoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, User model)
        {
            try
            {
                var check  = await _userService.Delete(model);
                if (check == false)
                {
                    _logger.Error($"User with id {id} not deleted!");
                    return RedirectToAction("EmptyList");
                }
                _logger.Warning($"Usunieto uzytkownika!");
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EmptyList()
        {
            return View();
        }

        public ActionResult AddTransaction(string id)
        {
            return RedirectToAction("Create", "TransactionUser", new { id });
        }
        public ActionResult ViewTransaction(string id)
        {
            return RedirectToAction("Index", "Chart", new { id });
        }
    }
}
