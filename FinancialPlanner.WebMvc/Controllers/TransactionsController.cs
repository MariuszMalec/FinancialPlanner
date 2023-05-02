using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;

        public TransactionsController(ApplicationDbContext context, IMapper mapper = null, ITransactionService transactionService = null)
        {
            _context = context;
            _mapper = mapper;
            _transactionService = transactionService;
        }

        // GET: Transactions
        public async Task<IActionResult> Index(
            CategoryOfTransaction category,
            TypeOfTransaction type, 
            string description, 
            DateTime dateFrom, 
            DateTime dateTo, 
            string sortAmount)
        {
            ViewData["AmountSortParam"] = sortAmount == "Amount" ? "amount_desc" : "Amount";
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Amount" : "";

            var transactions = await _transactionService.GetAllQueryable();

            transactions = _transactionService.FilterByTypeCategory(transactions, type, category);
            transactions = _transactionService.FilterByDates(transactions, dateFrom, dateTo);
            transactions = _transactionService.FilterByDescription(transactions, description);

            var sorted = from s in transactions
                                 select s;
            switch (sortAmount)
            {
                case "Amount":
                    sorted = sorted.OrderByDescending(s => s.Amount);
                    break;
                default:
                    sorted = sorted.OrderBy(s => s.Amount);
                    break;
            }
            var model = new TransactionSearchDto() {
                Category = category,
                Transactions = sorted, 
                Type = type,
                Description = description,
                DateFrom = dateFrom,
                DateTo = dateTo 
            };

            return View(model);
        }

        public async Task<IActionResult> GetMonthlyIncomeAndExpenses()
        {
            var transactions = await _transactionService.GetAllQueryable();
            var model = await  _transactionService.FilterByYearBalance(transactions);
            if (model == null)
            {
                return NotFound();
            }
            if (model.Count() == 0)
            {
                return NotFound("Transactions not found!");
            }
            return View(model);
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
                return NotFound();
            }

            return View(model);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Currency,Type,Category,Amount,BalanceAfterTransaction,Description,Date,UserId,Id,CreatedAt")] Logic.Models.Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
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
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(string id)
        {
          return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
