using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Interfaces;

namespace FinancialPlanner.Logic.Repository
{
    public interface IRepository<T> where T : IEntity
    {
        Task<IEnumerable<T>> GetAll();
        IQueryable<T> GetAllQueryable();
        Task<T> GetById(string id);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
