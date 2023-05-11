using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Exceptions;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FinancialPlanner.Logic.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository<Transaction> _repository;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public TransactionService(IRepository<Transaction> repository, ApplicationDbContext context, ILogger logger, IMapper mapper = null, IUserService userService = null)
        {
            _repository = repository;
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IList<Transaction>> GetAll()
        {
            var transactions = _context.Transactions.ToList();
            if (!transactions.Any())
            {
                throw new NotFoundException("Transactions not found");
            }
            return transactions;
        }

        public async Task<IQueryable<Transaction>> GetAllQueryable()
        {
            var all = _context.Set<Transaction>().Include(s => s.User);//TODO czy da sie usera dodac do repository??

            if (!all.Any())
            {
                throw new NotFoundException("Transactions not found");
            }
            _logger.Information($"Get all transaction with user at {DateTime.Now}");
            return all;
        }

        public async Task<TransactionUserDto> GetById(string id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            var currentTransaction = _mapper.Map<TransactionUserDto>(transaction);
            _logger.Information($"Get By Id {id} transaction with user at {DateTime.Now}");
            return currentTransaction;

            //TODO how to add to repo this
            //return await _repository.GetById(id);
        }
        public async Task Insert(string id, TransactionUserDto model)
        {
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
            //_context.Transactions.Add(transaction);
            //_context.SaveChanges();
            await _repository.Insert(transaction);

            //zapisac nowy balance w user
            var user = await _userService.GetById(id);
            user.Balance = transaction.BalanceAfterTransaction;
            _logger.Information($"Insert {id} transaction for user at {DateTime.Now}");
            await _userService.Update(user);
        }
        public async Task<bool> Delete(string id)
        {
            var userId = _context.Transactions.Where(t => t.Id == id).Select(t => t.UserId).FirstOrDefault();
            var getBalance = _context.Users.Where(u => u.Id == userId).Select(u => u.Balance).FirstOrDefault();
            var getAmount = _context.Transactions.Where(t => t.Id == id).Select(t => t.Amount).FirstOrDefault();
            var getType = _context.Transactions.Where(t => t.Id == id).Select(t => t.Type).FirstOrDefault();
            var newBalance = getType == Logic.Enums.TypeOfTransaction.Income ? (getBalance - getAmount) : (getBalance + getAmount);

            var transaction = _context.Transactions.Where(t => t.Id == id).FirstOrDefault();
            _context.Transactions.Remove(transaction);
            _context.SaveChanges();

            var user = await _userService.GetById(userId);
            user.Balance = newBalance;
            await _userService.Update(user);
            _logger.Warning($"Delete By Id {id} transaction with user at {DateTime.Now}");
            return true;
        }

        public IQueryable<Transaction> FilterByDates(IQueryable<Transaction> transactions, DateTime dateFrom, DateTime dateTo)
        {
            if (dateTo == new DateTime(0001, 01, 01))
            {
                dateTo = DateTime.Now;
            }

            transactions = transactions.Where(t => t.CreatedAt >= dateFrom && t.CreatedAt <= dateTo);
            _logger.Information($"Filter By transaction at {DateTime.Now}");
            return transactions;
        }

        public IQueryable<Transaction> FilterByDescription(IQueryable<Transaction> transactions, string description)
        {
            if (description is not null)
            {
                _logger.Information($"FilterByDescription {description} transaction at {DateTime.Now}");
                return transactions.Where(t => t.Description.ToLower().Contains(description.ToLower()));
            }
            return transactions;
        }
        public IQueryable<Transaction> FilterByTypeCategory(IQueryable<Transaction> transactions, TypeOfTransaction type, CategoryOfTransaction category)
        {
            _logger.Information($"FilterByTypeCategory By transaction at {DateTime.Now}");
            if (transactions.Count() > 0 && type == TypeOfTransaction.Outcome && (category == CategoryOfTransaction.allOutcome || category == CategoryOfTransaction.All))
            {
                return transactions.Where(t => t.Type == type)                      ;
            }
            if (transactions.Count() > 0 && type == TypeOfTransaction.Income && (category == CategoryOfTransaction.allIncome || category == CategoryOfTransaction.All))
            {
                return transactions.Where(t => t.Type == type)
                                   ;
            }
            if (transactions.Count() > 0 && type != TypeOfTransaction.All)
            {
                return transactions.Where(t => t.Type == type)
                                   .Where(t => t.Category == category)
                                   ;
            }
            return transactions;
        }
        public async Task<IEnumerable<MonthlyIncomeAndExpenses>> FilterByYearBalance(IQueryable<Transaction> transactions)
        {
            _logger.Information($"FilterByYearBalance By transaction at {DateTime.Now}");
            var monthsToDate = Enumerable.Range(1, 12)
                                .Select(m => new
                                {
                                    firstDay = new DateTime(DateTime.Today.Year, m, 1),
                                    endDay = new DateTime(DateTime.Today.Year, m, DateTime.DaysInMonth(DateTime.Today.Year, m)),
                                    beforefirstDay = new DateTime(DateTime.Today.Year, m, 1).AddMonths(-1).AddDays(0),
                                    beforelastDay = new DateTime(DateTime.Today.Year, m, 1).AddDays(-1),
                                    currentDifference = FilterByDates(transactions, new DateTime(DateTime.Today.Year, 1, 1), new DateTime(DateTime.Today.Year, m, DateTime.DaysInMonth(DateTime.Today.Year, m)))
                                                       .Where(t => t.Type == TypeOfTransaction.Income).Select(x => x.Amount).Sum() -
                                                       FilterByDates(transactions, new DateTime(DateTime.Today.Year, 1, 1), new DateTime(DateTime.Today.Year, m, DateTime.DaysInMonth(DateTime.Today.Year, m)))
                                                      .Where(t => t.Type == TypeOfTransaction.Outcome).Select(x => x.Amount).Sum(),
                                    
                                })
                                .ToList();

            var sums = from month in monthsToDate
                       select new MonthlyIncomeAndExpenses
                       {
                           Month = month.firstDay,
                           Income = FilterByDates(transactions,month.firstDay, month.endDay)
                           .Where(t => t.Type == TypeOfTransaction.Income).Select(x => x.Amount).Sum()
                           ,
                           Expenses = FilterByDates(transactions, month.firstDay, month.endDay)
                           .Where(t => t.Type == TypeOfTransaction.Outcome).Select(x => x.Amount).Sum(),
                           Balance = month.currentDifference
                       };
            return sums;
        }

        public IEnumerable<Transaction> FilterTransactionByMounth(IQueryable<Transaction> transactions, int mounth)
        {
            var firstDay = new DateTime(DateTime.Today.Year, mounth, 1);
            var endDay = new DateTime(DateTime.Today.Year, mounth, DateTime.DaysInMonth(DateTime.Today.Year, mounth));
            var transactionsUser = transactions.Where(t => t.CreatedAt >= firstDay && t.CreatedAt <= endDay);
            _logger.Information($"FilterTransactionByMounth {mounth} transaction at {DateTime.Now}");
            return transactionsUser;
        }
    }
}
