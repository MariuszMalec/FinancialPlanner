using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Exceptions;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace FinancialPlanner.Logic.Services
{
    public class TransactionService
    {
        private readonly IRepository<Transaction> _repository;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TransactionService> _logger;
        private readonly IMapper _mapper;

        public TransactionService(IRepository<Transaction> repository, ApplicationDbContext context, ILogger<TransactionService> logger, IMapper mapper, IUserService userService = null)
        {
            _repository = repository;
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IList<Transaction>> GetAll()
        {
            var users = _context.Users.ToList();
            var transactions = _context.Transactions.ToList();
            if (!transactions.Any())
            {
                throw new NotFoundException("Transactions not found");
            }
            return transactions;
        }

        public async Task<IQueryable<Transaction>> GetAllQueryable()
        {
            var all = _context.Set<Transaction>().Include(s => s.User)
                ;//TODO czy da sie usera dodac do repository??

            if (!all.Any())
            {
                throw new NotFoundException("Transactions not found");
            }
            return all;
        }

        public async Task<TransactionUserDto> GetById(string id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            var currentTransaction = _mapper.Map<TransactionUserDto>(transaction);
            return currentTransaction;

            //TODO how to add to repo this
            //return await _repository.GetById(id);
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
    }
}
