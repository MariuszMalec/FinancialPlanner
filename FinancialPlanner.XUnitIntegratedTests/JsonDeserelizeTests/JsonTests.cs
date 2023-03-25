using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanner.XUnitIntegratedTests.JsonDeserelizeTests
{
    public class JsonTests
    {
        [Fact]
        public void TestCategory()
        {
            var transactions = LoadDataService<TransactionDto>.ReadTransacionFile();
            var category = transactions.Select(x=>x.Category).FirstOrDefault().ToString();
            Assert.Equal("Salary", category);

        }
    }
}
