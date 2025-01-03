﻿using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Exceptions;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using static System.Net.WebRequestMethods;

namespace FinancialPlanner.Logic.Services
{
    public class TransactionService : ITransactionService
    {
        private static readonly IList<Transaction> _transactions = LoadDataService<Transaction>.ReadTransacionFile();
        private readonly IRepository<Transaction> _repositoryTransaction;
		private readonly IUserService<User> _userService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ITransactionService> _logger;
        private readonly IMapper _mapper;

		public TransactionService(IRepository<Transaction> repository, ApplicationDbContext context, ILogger<ITransactionService> logger, IMapper mapper = null, IUserService<User> userService = null)
		{
			_repositoryTransaction = repository;
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
            _logger.LogInformation("Get all transaction with user at {registrationDate}", DateTime.Now);
            return all;
        }

        public async Task<TransactionUserDto> GetById(string id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                _logger.LogError($"Brak tranzakcji {id} !");
                return null;
            }
            var userId = transaction.UserId;
            var user = await _context.Users.FindAsync(userId);
            transaction.User = user;
            var currentTransaction = _mapper.Map<TransactionUserDto>(transaction);
            _logger.LogInformation($"GetById succesfull at {DateTime.Now}");
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
                CreatedAt = model.CreatedAt,
                Picture = _context.TransactionPictures.Where(x=>x.Category == model.Category).Select(x=>x.Source).FirstOrDefault(),
                TransactionPicture = _context.TransactionPictures.Where(x => x.Category == model.Category).Select(x => x).FirstOrDefault()
            };
            //_context.Transactions.Add(transaction);
            //_context.SaveChanges();
            await _repositoryTransaction.Insert(transaction);

            //zapisac nowy balance w user
            var user = await _userService.GetById(id);
            user.Balance = transaction.BalanceAfterTransaction;
            await _userService.Update(user);
        }

        public string AddPicture(CategoryOfTransaction categoryOfTransaction)//to mozna pobierac z bazy
        {
            if (categoryOfTransaction == CategoryOfTransaction.Kids)
            {
                return "https://plus.unsplash.com/premium_photo-1686836995180-06df3b20884e?auto=format&fit=crop&q=60&w=500&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NXx8a2lkc3xlbnwwfHwwfHx8MA%3D%3D";
            }
            return string.Empty;
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
            return true;
        }

        public IQueryable<Transaction> FilterByDates(IQueryable<Transaction> transactions, DateTime dateFrom, DateTime dateTo)
        {
            if (dateTo == new DateTime(0001, 01, 01))
            {
                dateTo = DateTime.Now;
            }

            transactions = transactions.Where(t => t.CreatedAt >= dateFrom && t.CreatedAt <= dateTo);
            _logger.LogInformation("FilterByDates successful at {registrationDate}", DateTime.Now);
            return transactions;
        }

        public IQueryable<Transaction> FilterByDescription(IQueryable<Transaction> transactions, string description)
        {
            if (description is not null)
                return transactions.Where(t => t.Description.ToLower().Contains(description.ToLower()));
            _logger.LogInformation("FilterByDescription successful at {registrationDate}", DateTime.Now);
            return transactions;
        }
        public IQueryable<Transaction> FilterByTypeCategory(IQueryable<Transaction> transactions, TypeOfTransaction type, CategoryOfTransaction category)
        {
            if (transactions.Count() > 0 && type == TypeOfTransaction.Outcome && (category == CategoryOfTransaction.allOutcome || category == CategoryOfTransaction.All))
            {
                return transactions.Where(t => t.Type == type)
                                   ;
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
            _logger.LogInformation("FilterByYearBalance successful at {registrationDate}", DateTime.Now);
            return sums;
        }

        public IEnumerable<Transaction> FilterTransactionByMounth(IQueryable<Transaction> transactions, DateTime date)
        {
            var firstDay = new DateTime(date.Year, date.Month, 1);
            var endDay = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(DateTime.Today.Year, date.Month));
            var transactionsUser = transactions.Where(t => t.CreatedAt >= firstDay && t.CreatedAt <= endDay);
            _logger.LogInformation("FilterTransactionByMounth successful at {registrationDate}", DateTime.Now);
            return transactionsUser;
        }

        public async Task Edit(string id, TransactionUserDto model)
        {
            var getAmountFromDataBase = _context.Transactions.Where(u => u.Id == model.Id).Select(u => u.Amount).FirstOrDefault();
            var newAmount = (getAmountFromDataBase - model.Amount);
            var getBalance = _context.Users.Where(u => u.Id == model.UserId).Select(u => u.Balance).FirstOrDefault();
            var getAmount = model.Type == Logic.Enums.TypeOfTransaction.Income ? (getBalance - newAmount) : (getBalance + newAmount);
            var getUser = await _userService.GetById(model.UserId);

            var transaction = new Transaction()
            {
                Id = id,
                User = getUser, 
                UserId = model.UserId,
                Currency = model.Currency,
                Type = model.Type,
                Category = model.Category,
                Amount = model.Amount,
                BalanceAfterTransaction = getAmount,
                Description = model.Description,
                CreatedAt = model.CreatedAt,
                Picture = _context.TransactionPictures.Where(x => x.Category == model.Category).Select(x => x.Source).FirstOrDefault(),
                TransactionPicture = _context.TransactionPictures.Where(x => x.Category == model.Category).Select(x => x).FirstOrDefault()
            };
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();

            var user = await _userService.GetById(model.UserId);
            user.Balance = transaction.BalanceAfterTransaction;
            await _userService.Update(user);
            _logger.LogInformation("Transaction was edited successful at {registrationDate}", id, DateTime.Now);
        }

        public async Task Update(string id, Transaction model)//dodane bo nie dziala edit
        {
			var getUser = await _repositoryTransaction.GetAllQueryable().Where(x => x.Id == id).Select(x => x.User).FirstOrDefaultAsync();

			var getAmountFromDataBase = _context.Transactions.Where(u => u.Id == model.Id).Select(u => u.Amount).FirstOrDefault();
            var newAmount = (getAmountFromDataBase - model.Amount);
            var getBalance = _context.Users.Where(u => u.Id == getUser.Id).Select(u => u.Balance).FirstOrDefault();
            var getAmount = model.Type == Logic.Enums.TypeOfTransaction.Income ? (getBalance - newAmount) : (getBalance + newAmount);

            var transaction = new Transaction()
            {
                Id = id,
                User = getUser,
                UserId = getUser.Id,
                Currency = model.Currency,
                Type = model.Type,
                Category = model.Category,
                Amount = model.Amount,
                BalanceAfterTransaction = getAmount,
                Description = model.Description,
                CreatedAt = model.CreatedAt,
                Picture = _context.TransactionPictures.Where(x => x.Category == model.Category).Select(x => x.Source).FirstOrDefault(),
                TransactionPicture = _context.TransactionPictures.Where(x => x.Category == model.Category).Select(x => x).FirstOrDefault()
            };
            //_context.Transactions.Update(transaction);
            //await _context.SaveChangesAsync();
            if (transaction.UserId == null)
            {
                _logger.LogError("Transaction was not edited successful at {registrationDate}!", id, DateTime.Now);
                return;
            }

            transaction.User.Balance = transaction.BalanceAfterTransaction;

			await _repositoryTransaction.Update(transaction);

            _logger.LogInformation("Transaction was edited successful at {registrationDate}", id, DateTime.Now);
        }
    }
}
