using FinancialPlanner.Logic.Models;

namespace FinancialPlanner.Logic.Interfaces
{
    public interface IUserService<T>
    {
        Task<IEnumerable<T>> GetAll();

        Task<IQueryable<T>> GetAllQueryable();

        IEnumerable<User> GetAllUser();

        Task<bool> Insert(T user);
		Task<User> InsertUser(T user);

		Task<T> GetById(string id);

        Task<bool> Delete(T user);

        Task Update(T user);

        bool GetByEmail(string userEmail);
    }
}
