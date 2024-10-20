﻿using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.ExtentionsMethod;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private ILogger _logger;

        public TransactionsController(ApplicationDbContext context, IMapper mapper = null, ITransactionService transactionService = null, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _transactionService = transactionService;
            _logger = logger;
        }

        // GET: Transactions
        public async Task<IActionResult> Index(
            CategoryOfTransaction category,
            TypeOfTransaction type, 
            string description, 
            DateTime dateFrom, 
            DateTime dateTo, 
            string sortOrder)
        {

                //ViewData["AmountSortParam"] = sortAmount == "Amount" ? "amount_desc" : "Amount";
                //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Amount" : "";

                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = System.String.IsNullOrEmpty(sortOrder) ? "Amount" : "";
                ViewBag.DateSortParm = sortOrder == "CreatedAt" ? "date_desc" : "CreatedAt";


                var transactions = await _transactionService.GetAllQueryable();

                transactions = _transactionService.FilterByTypeCategory(transactions, type, category);
                transactions = _transactionService.FilterByDates(transactions, dateFrom, dateTo);
                transactions = _transactionService.FilterByDescription(transactions, description);

                var sorted = from s in transactions
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
                    default:
                        sorted = sorted.OrderBy(s => s.Amount);
                        break;
                }
                var model = new TransactionSearchDto()
                {
                    Category = category,
                    Transactions = sorted,
                    Type = type,
                    Description = description,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                };
                
                if (type == TypeOfTransaction.Outcome)
                    ViewData["SearchingAmount"] = model.Transactions.Where(x=>x.Type == TypeOfTransaction.Outcome).Select(x=>x.Amount).Sum();
                if (type == TypeOfTransaction.Income)
                    ViewData["SearchingAmount"] = model.Transactions.Where(x=>x.Type == TypeOfTransaction.Income).Select(x=>x.Amount).Sum();

                _logger.Information("Load all transactions successfully at {registrationDate}", DateTime.Now);
                return View(model);
                //return PartialView("_TransactionSearchForm", model);
        }

        public async Task<IActionResult> GetMonthlyIncomeAndExpenses()
        {
            if (ExtentionsMethod.IsAjaxRequest(this.Request))
            {
                var transactions = await _transactionService.GetAllQueryable();
                var model = await _transactionService.FilterByYearBalance(transactions);
                if (model == null)
                {
                    return NotFound();
                }
                if (model.Count() == 0)
                {
                    return NotFound("Transactions not found!");
                }
                _logger.Information("Load user expanses by month successfully at {registrationDate}", DateTime.Now);
                //return View(model);
                Thread.Sleep(1500);
                return PartialView("_Report", model);
            }
            else
            {
                return View("Report");
            }
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var transaction = await _transactionService.GetById(id);

            var model = _mapper.Map<Transaction>(transaction);

            if (model == null)
            {
                _logger.Error("Brak transaction {id}, {registrationDate}", DateTime.Now);
                return NotFound($"Brak transaction {id}");
            }
            _logger.Information("Load transaction details successfully at {registrationDate}", DateTime.Now);
            return View(model);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Transactions == null)
            {
                _logger.Error("Not found transaction {id}, {registrationDate}", DateTime.Now);
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                _logger.Error("Not found transaction {id}, {registrationDate}", DateTime.Now);
                return NotFound();
            }

            var model = await _transactionService.GetById(id);
            ViewData["FullName"] = $"{model.FirstName} {model.LastName}";
            var getUserId = model.UserId;
            ViewData["UserId"] = getUserId;
            ViewData["UserTransactionId"] = id;

            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Transaction model) //nie wiem po co to dalem ?[Bind("Currency,Type,Category,Amount,BalanceAfterTransaction,Description,Date,UserId,Id,CreatedAt")] Logic.Models.Transaction transaction)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _transactionService.Update(id, model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _logger.Information("Edit transaction successfully at {registrationDate}", DateTime.Now);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private static TransactionUserDto MapToTransactionUserDto(Transaction transaction)
        {
            return new TransactionUserDto { 
                Id = transaction.Id ,
                FirstName =transaction.User.FirstName, 
                LastName =transaction.User.LastName,

            };
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                _logger.Error("Not found transaction {id}, {registrationDate}", DateTime.Now);
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Transactions'  is null.");
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            _logger.Error("Delete transaction with id {id}, {registrationDate}", DateTime.Now);
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(string id)
        {
          return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
