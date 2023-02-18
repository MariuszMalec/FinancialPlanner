using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;

namespace FinancialPlanner.Logic.Services
{
    public class UserService : IUserService
    {
        private static readonly List<User> _users = LoadDataService.ReadUserFile();

        public Task Delete(User user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            return _users.ToList();
        }

        public Task<User> GetByEmail(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Insert(User user)
        {
            throw new NotImplementedException();
        }

        public Task Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
