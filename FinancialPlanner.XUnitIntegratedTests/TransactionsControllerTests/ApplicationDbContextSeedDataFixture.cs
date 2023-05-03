using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialPlanner.XUnitIntegratedTests.TransactionsControllerTests
{
    public class ApplicationDbContextSeedDataFixture : IDisposable
    {
        public ApplicationDbContext Context { get; private set; }

        public ApplicationDbContextSeedDataFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TransactionDb")
                .Options;

            Context = new ApplicationDbContext(options);

            Context.Transactions.Add(new Transaction
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                Amount = 0,
                BalanceAfterTransaction = 0,
                Category = Logic.Enums.CategoryOfTransaction.Salary,
                Type = Logic.Enums.TypeOfTransaction.Income,
                Currency = Logic.Enums.Currency.PLN,
                Description = "W morde kredyt",
                CreatedAt = DateTime.Now,
                UserId = Guid.NewGuid().ToString(),
            });
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
