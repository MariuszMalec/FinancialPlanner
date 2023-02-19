using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Repository;

namespace FinancialPlanner.Logic.Services
{
    public class UserService : IUserService
    {
        private static readonly List<User> _users = LoadDataService.ReadUserFile();
        private readonly IRepository<User> _repository;

        public UserService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public Task Delete(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
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

        public Task Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
