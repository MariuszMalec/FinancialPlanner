using AutoMapper;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.WebMvc.Controllers;
using FinancialPlanner.WebMvc.Profiles;
using Microsoft.AspNetCore.Mvc;
using FinancialPlanner.Logic.Context;
using Moq;

namespace FinancialPlanner.XUnitIntegratedTests.TransactionsControllerTests
{
    public class TransactionsControllerTests : IClassFixture<ApplicationDbContextSeedDataFixture>
    {
        ApplicationDbContextSeedDataFixture _fixture;
        public TransactionsControllerTests(ApplicationDbContextSeedDataFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetDetailsTransaction_ReturnTrue_WhenModelPass()
        {
            // Arrange
            var newGuid = Guid.NewGuid().ToString();
            var mockRepo = new Mock<ITransactionService>();
            mockRepo.Setup(r => r.GetById(It.IsAny<string>()))
               .Returns(GetTransactionUserDto(newGuid));

            var myProfile = new UserViewProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var controller = new TransactionsController(_fixture.Context, mapper, mockRepo.Object);

            // Act
            var result = await controller.Details(newGuid);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Transaction>(viewResult.ViewData.Model);
            Assert.Equal(newGuid, model.Id);
        }

        private async Task<TransactionUserDto> GetTransactionUserDto(string guid)
        {
            return new TransactionUserDto()
            {
                Id = guid,
                Amount = 0,
                BalanceAfterTransaction = 0,
                Description = "test",
                Category = Logic.Enums.CategoryOfTransaction.Car,
                Type = Logic.Enums.TypeOfTransaction.Outcome,
                CreatedAt = DateTime.UtcNow,
                Currency = Logic.Enums.Currency.PLN,
                UserId = Guid.NewGuid().ToString(),
                Balance= 0,
                FirstName= "test",
                LastName = "test",
            };
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
