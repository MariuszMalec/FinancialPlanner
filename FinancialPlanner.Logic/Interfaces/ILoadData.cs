namespace FinancialPlanner.Logic.Interfaces
{
    public interface ILoadData<T>
    {
        IList<T> GetAll(string file);
    }
}
