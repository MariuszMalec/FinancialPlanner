using FinancialPlanner.Logic.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;

namespace FinancialPlanner.XUnitIntegratedTests.MvcTests
{
    public class TransactionsControllerTests : IClassFixture<FinancialWebAppFactory<Program>>
    {
        private HttpClient _client;

        WebApplicationFactory<Program> factory = new WebApplicationFactory<Program>();

        public TransactionsControllerTests(FinancialWebAppFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:7293/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }

        private async Task<IList<Transaction>> GetTransactions(string guid)
        {
            var transactions = new List<Transaction>()
            {
                new Transaction () 
                { 
                    Id=guid,
                    Amount=0,
                    BalanceAfterTransaction =0,
                    Date=DateTime.UtcNow,
                    Description="test",
                    Category = Logic.Enums.CategoryOfTransaction.Car,
                    Type = Logic.Enums.TypeOfTransaction.Outcome,
                    CreatedAt=DateTime.UtcNow,
                    Currency = Logic.Enums.Currency.PLN,
                    User = new User() {},
                    UserId= Guid.NewGuid().ToString(),
                }
            };
            return transactions;
        }
    }
}
