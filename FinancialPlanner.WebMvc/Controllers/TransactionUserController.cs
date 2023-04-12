using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System.Globalization;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class TransactionUserController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly TransactionService _transactionService;
        private readonly IUserService _userService;

        public TransactionUserController(ApplicationDbContext context, IMapper mapper = null, TransactionService transactionService = null, IUserService userService = null)
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

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _transactionService.GetById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: UserController/Create
        public ActionResult Create(string id)
        {
            ViewData["UserId"] = id;
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
            
            //TODO dodac to do serwisu
            var getBalance = _context.Users.Where(u => u.Id == model.Id).Select(u => u.Balance).FirstOrDefault();
            var getAmount = model.Type == Logic.Enums.TypeOfTransaction.Income ? (getBalance + model.Amount) : (getBalance - model.Amount);
            var transaction = new Transaction()
            {
                UserId = id,
                Currency = model.Currency,
                Type = model.Type,
                Category = model.Category,
                Amount = model.Amount,
                BalanceAfterTransaction = getAmount,
                Description = model.Description,
                CreatedAt = model.CreatedAt
            };
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            //zapisac nowy balance w user
            var user = await _userService.GetById(id);
            user.Balance = transaction.BalanceAfterTransaction;
            _userService.Update(user);

            //mapowanie na TransactionUserDto

            //return RedirectToAction(nameof(Index), new { id });
            return RedirectToAction("GetUserTransactions", "User", new { model.Id, model.UserId });
        }

        public async Task<ActionResult> Edit(string id)
        {
            var model = await _transactionService.GetById(id);
            ViewData["FullName"] = $"{model.FirstName} {model.LastName}";
            if (model == null)
            {
                return NotFound($"Not found user with {id}");
            }
            return View(model);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, TransactionUserDto model)
        {
            if (model.UserId == null)
            {
                return NotFound("No user!");
            }

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
            _userService.Update(user);

            //return RedirectToAction(nameof(Index));
            return RedirectToAction("GetUserTransactions", "User", new { model.Id,  model.UserId });
        }

        public async Task<ActionResult> Delete(string id)
        {
            var model = await _context.Transactions.FindAsync(id);

            if (model == null)
            {
                return NotFound($"Not found user with {id}");
            }
            return View(model);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, Transaction model)
        {
            if (id == null)
            {
                return NotFound("No transaction!");
            }

            var userId = _transactionService.GetById(id).Result.UserId;

            var check = await _transactionService.Delete(id);
            if (!check)
            {
                return BadRequest("transaction not deleted!");
            }

            //return RedirectToAction(nameof(Index));
            return RedirectToAction("GetUserTransactions", "User", new { model.Id, userId });
        }
    }
}
