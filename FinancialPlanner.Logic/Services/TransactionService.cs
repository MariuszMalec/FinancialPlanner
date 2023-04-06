using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Exceptions;
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
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TransactionService> _logger;
        private readonly IMapper _mapper;

        public TransactionService(IRepository<Transaction> repository, ApplicationDbContext context, ILogger<TransactionService> logger, IMapper mapper)
        {
            _repository = repository;
            _context = context;
            _logger = logger;
            _mapper = mapper;
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
                ;//TODO czy da sie to dodac do repository??

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
    }
}
