using FinancialPlanner.Logic.Models;

namespace FinancialPlanner.Logic.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();

        Task<IQueryable<User>> GetAllQueryable();

        Task Insert(User user);

        Task<User> GetById(string id);

        Task<bool> Delete(User user);

        Task Update(User user);

        Task<User> GetByEmail(string userEmail);
    }
}
