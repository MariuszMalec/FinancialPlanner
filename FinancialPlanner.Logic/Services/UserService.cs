using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Repository;
using Microsoft.EntityFrameworkCore;

namespace FinancialPlanner.Logic.Services
{
    public class UserService : IUserService
    {
        private static readonly List<User> _users = LoadDataService.ReadUserFile();
        private readonly IRepository<User> _repository;
        private readonly ApplicationDbContext _context;

        public UserService(IRepository<User> repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
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
            return _repository.Insert(user);
        }

        public async Task Update(User user)
        {
            await _repository.Update(user);
        }
    }
}
