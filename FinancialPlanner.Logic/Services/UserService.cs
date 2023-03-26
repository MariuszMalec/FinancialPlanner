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
    public class UserService : IUserService
    {
        private static readonly List<User> _users = LoadDataService<User>.ReadUserFile();
        private readonly IRepository<User> _repository;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(IRepository<User> repository, ApplicationDbContext context, ILogger<UserService> logger = null)
        {
            _repository = repository;
            _context = context;
            _logger = logger;
        }

        public Task Delete(User user)
        {
            return _repository.Delete(user);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var all = _context.Set<User>().Include(e=>e.Role);//TODO czy da sie to dodac do repository??

            return await _repository.GetAll();
        }

        public IQueryable<User> GetAllQueryable()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByEmail(string userEmail)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetById(string id)
        {
            return await _repository.GetById(id);
        }

        public Task Insert(User user)
        {
            //TODO dodanie istniejacej roli do uzytkownika
            var role = _context.Roles.Where(r => r.Name == "User").FirstOrDefault();
            user.Role = role;
            var encodePassword = Base64EncodeDecode.Base64Encode("trudnehaslo");
            user.PasswordHash = encodePassword;
            //TODO validations in service
            var check = UserValidate.Check(user, _context);//TODO uzycie middleware
            if (check != string.Empty)
            {
                _logger.LogError($"404! user not created! Blad walidacji, {check}");
                throw new BadRequestException($"404! user not created! Blad walidacji, {check}");               
            }
            return _repository.Insert(user);
        }

        public async Task Update(User user)
        {
            await _repository.Update(user);
        }
    }
}
