using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FinancialPlanner.Logic.Services;
using Microsoft.EntityFrameworkCore;
using FinancialPlanner.Logic.Repository;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly TransactionService _transactionService;
        private readonly IRepository<User> _repository;

        public UserController(IUserService userService, IMapper mapper = null, TransactionService transactionService = null, IRepository<User> repository = null)
        {
            _userService = userService;
            _mapper = mapper;
            _transactionService = transactionService;
            _repository = repository;
        }

        // GET: UserController

        public async Task<IActionResult> GetUserTransactions(string id)
        {
            var transactions = await _transactionService.GetAllQueryable();

            var userTransactions = transactions.Where(u=>u.User.Id == id).ToList();

            var model = _mapper.Map<List<TransactionUserDto>>(userTransactions);

            return model != null ?
                          View(model) :
                          Problem("Entity set 'ApplicationDbContext.Transactions'  is null.");
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _transactionService.GetAll();//TODO najperw aktualizacja balansu dla userow wg transakcji

            var users = await _userService.GetAllQueryable();//TODO dodalem do usera role!!

            if (!users.Any())
            {
                return View("No users!");
            }

            //TODO automapper chyba gorszy bo trzeba szukac profile
            var model = _mapper.Map<List<UserDto>>(users);

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

            return View(model);
        }

        // GET: UserController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var user = await _userService.GetById(id);

            if (user == null)
            {
                return BadRequest($"Brak uzytkownika {id}");
            }

            var model = new UserDto()
            {

            };

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
                    return NotFound("404 bledny model!");
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
                    return NotFound($"User was not created!");

                return RedirectToAction(nameof(Index));
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var model = await _userService.GetById(id);
            if (model == null)
            {
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
                return RedirectToAction(nameof(Index));
        }

        // GET: UserController/Delete/5
        public async Task<ActionResult<User>> Delete(string id)
        {
            var model = await _userService.GetById(id);
            if (model == null)
            {
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
                    return RedirectToAction("EmptyList");
                }
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
    }
}
