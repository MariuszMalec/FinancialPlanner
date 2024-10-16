﻿using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Exceptions;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Repository;
using FinancialPlanner.Logic.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinancialPlanner.Logic.Services
{
    public class UserService : IUserService<User>
    {
        private static readonly List<User> _users = LoadDataService<User>.ReadUserFile();
        private readonly IRepository<User> _repository;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> repository, ApplicationDbContext context, ILogger<UserService> logger = null, IMapper mapper = null)
        {
            _repository = repository;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> Delete(User user)
        {
            //TODO validations in service
            var check = UserValidate.Delete(user, _context);//TODO uzycie middleware
            if (check != string.Empty)
            {
                _logger.LogError($"404! user can't be deleted!, {check}");
                return false;
            }
            //TODO nalezy usunac wszystkie tranzakcje uzytkownika
            var transactions = _context.Transactions.Where(t => t.User.Id == user.Id);
            foreach (var transaction in transactions)
            {
                _context.Transactions.Remove(transaction);
            }
            await _context.SaveChangesAsync();
            //TODO dopiero teraz usunac usera
            await _repository.Delete(user);
            return true;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var all = _context.Set<User>().Include(e => e.Role);//TODO czy da sie to dodac do repository??
            if (!_context.Users.Any())
            {
                throw new NotFoundException("Users not found");
            }
            return await _repository.GetAll();
        }

        public IEnumerable<User> GetAllUser()//TODO add for blazor , don't work correctly async function getall?
        {
            var users = _context.Users.ToList();
            return users;
        }

        public async Task<IQueryable<User>> GetAllQueryable()
        {
            //var all = _context.Set<User>().Include(e => e.Role);//TODO czy da sie to dodac do repository??
            var all = _repository.GetAllQueryable().Include(u => u.Role);//TODO cos takiego
            if (!all.Any())
            {
                throw new NotFoundException("Users not found");
            }
            return all;
        }

        public bool GetByEmail(string userEmail)
        {
            var existUser = _repository.GetAll().Result.Where(u => u.Email == userEmail).Select(u => u).FirstOrDefault();
            if (existUser != null)
            {
                return true;
            }
            return false;
		}

        public async Task<User> GetById(string id)
        {
            return await _repository.GetById(id);
        }
		public async Task<User> InsertUser(User user)
		{
			//TODO dodanie istniejacej roli do uzytkownika
			var role = _context.Roles.Where(r => r.Name == "User").FirstOrDefault();
			user.Role = role;
			var encodePassword = Base64EncodeDecode.Base64Encode("trudnehaslo");
			user.PasswordHash = encodePassword;
			//TODO validations in service           
			if (user.Age == null || user.Balance == null || user.Email == null || user.FirstName == null || user.LastName == null)//uzycie false bez maiddleware, co lepsze!
			{
				_logger.LogError($"404! user not created! Blad walidacji!");
				return new User();
			}

			var check = UserValidate.Create(user, _context);//TODO uzycie middleware
			if (check != string.Empty)
			{
				_logger.LogError($"404! user not created! Blad walidacji, {check}");
				throw new BadRequestException($"404! user not created! Blad walidacji, {check}");
			}
			await _repository.Insert(user);
            return user;
		}

        public async Task<bool> Insert(User user)
        {
            //TODO dodanie istniejacej roli do uzytkownika
            var role = _context.Roles.Where(r => r.Name == "User").FirstOrDefault();
            user.Role = role;
            var encodePassword = Base64EncodeDecode.Base64Encode("trudnehaslo");
            user.PasswordHash = encodePassword;
            //TODO validations in service           
            if (user.Age == null || user.Balance == null || user.Email == null || user.FirstName == null || user.LastName == null)//uzycie false bez maiddleware, co lepsze!
            {
                _logger.LogError($"404! user not created! Blad walidacji!");
                return false;
            }

            var check = UserValidate.Create(user, _context);//TODO uzycie middleware
            if (check != string.Empty)
            {
                _logger.LogError($"404! user not created! Blad walidacji, {check}");
                throw new BadRequestException($"404! user not created! Blad walidacji, {check}");               
            }
            await _repository.Insert(user);
            return true;
        }

        public async Task Update(User user)
        {
            //TODO validations in service
            var check = UserValidate.Edit(user, _context);//TODO uzycie middleware
            if (check != string.Empty)
            {
                _logger.LogError($"404! user not edited! Blad walidacji, {check}");
                throw new BadRequestException($"404! user not created! Blad walidacji, {check}");
            }

            //var transactions = await _context.Transactions.Where(t => t.UserId == user.Id).Select(t=>t.BalanceAfterTransaction).ToListAsync();
            //user.Balance = transactions.LastOrDefault();

            await _repository.Update(user);
        }
    }
}
