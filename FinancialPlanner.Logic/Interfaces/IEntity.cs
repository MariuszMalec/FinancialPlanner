namespace FinancialPlanner.Logic.Interfaces
{
    public interface IEntity
    {
        string Id { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
