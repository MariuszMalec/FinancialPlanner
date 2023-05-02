using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Models;

namespace FinancialPlanner.Logic.Interfaces
{
    public interface ITransactionService
    {
        Task<bool> Delete(string id);
        IQueryable<Transaction> FilterByDates(IQueryable<Transaction> transactions, DateTime dateFrom, DateTime dateTo);
        IQueryable<Transaction> FilterByDescription(IQueryable<Transaction> transactions, string description);
        Task<IEnumerable<MonthlyIncomeAndExpenses>> FilterByMonthlyBalance(int mounth);
        IQueryable<Transaction> FilterByTypeCategory(IQueryable<Transaction> transactions, TypeOfTransaction type, CategoryOfTransaction category);
        Task<IList<Transaction>> GetAll();
        Task<IQueryable<Transaction>> GetAllQueryable();
        Task<TransactionUserDto> GetById(string id);
        Task Insert(string id, TransactionUserDto model);
        IEnumerable<Transaction> GetTransactionByMounth(int mounth, IEnumerable<Transaction> transactions);
    }
}