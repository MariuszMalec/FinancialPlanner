using AutoMapper;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.WebMvc.Controllers;
using FinancialPlanner.WebMvc.Profiles;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Serilog;

namespace FinancialPlanner.XUnitIntegratedTests.UserControllerTests
{
    public class UserControllerTests
    {
        private readonly string _currentUserId;
        private readonly string _currentTransactionId;

        public UserControllerTests()
        {
            _currentUserId = Guid.NewGuid().ToString();
            _currentTransactionId = Guid.NewGuid().ToString();
        }

        [Fact]
        public async Task Detail_ReturnTrue_WhenExist()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var user = new User()
            {
                Id = id,
                CreatedAt= DateTime.UtcNow,
                Company = "",
                FirstName = "Mariusz",
                LastName = "Malec",
                Email = "mm@example.com",
                IsActive = true,
                Age = 47,
                Balance = 3000m,
                Currency = Logic.Enums.Currency.PLN
            };

            var mockRepo = new Mock<IUserService>();
            mockRepo.Setup(r => r.GetById(id))
                .ReturnsAsync(user);

            var controller = new UserController(mockRepo.Object);

            // Act
            var result = controller.Details(id);

            // Assert
            Assert.Equal(id, user.Id);
        }

        [Fact]
        public async void GetUserTransactions_ReturnFalse_WhenUserTransactionsNotExist()//TODO naprawic to
        {
            //Arange
            var mockRepoUser = new Mock<IUserService>();
            mockRepoUser.Setup(r => r.GetAllQueryable().Result)
                .Returns(GetUsers());

            var mockRepoTransaction = new Mock<ITransactionService>();
            mockRepoTransaction.Setup(r => r.GetAllQueryable().Result)
               .Returns(GetTransactions());

            var myProfile = new UserViewProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var logger = new Mock<ILogger>();
            logger.Setup(c => c.Information(It.IsAny<string>()))
                 ;

            var controller = new UserController(mockRepoUser.Object, mapper, mockRepoTransaction.Object, logger.Object);

            var id = GetUsers().Select(x=>x.Id).FirstOrDefault();
            var userid = GetTransactions().Select(x => x.UserId).FirstOrDefault();

            // Act
            var result = await controller.GetUserTransactions(id, userid, string.Empty);//TODO user is null in cotroller?

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<List<TransactionUserDto>>(viewResult.Model);

            Assert.NotNull(result);
            Assert.Equal("Test", model.Select(x => x.FirstName).FirstOrDefault());
        }

        [Fact]
        public void Index_ReturnFalse_WhenUsersNotExist()
        {
            //Arange
            var mockRepo = new Mock<IUserService>();
            mockRepo.Setup(r => r.GetAllQueryable().Result)
                .Returns(GetUsers());

            var mockRepoTransaction = new Mock<ITransactionService>();
            mockRepoTransaction.Setup(r => r.GetAllQueryable().Result)
               .Returns(GetTransactions());

            var myProfile = new UserViewProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var logger = new Mock<ILogger>();
            logger.Setup(c => c.Information(It.IsAny<string>()))
                 ;

            var controller = new UserController(mockRepo.Object, mapper, mockRepoTransaction.Object, logger.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<Task<ActionResult<List<UserDto>>>>(result);
            var results = Assert.IsType<ViewResult>(viewResult.Result.Result);
            var model = Assert.IsType<List<UserDto>>(results.Model);

            Assert.Equal("Test", model.Select(x=>x.FirstName).FirstOrDefault());
        }

        private User GetUser()
        {
            return new User
                {
                    Id = _currentUserId,
                    FirstName= "Test",
                    LastName= "Test",
                    Company="",
                    Balance=4000m,
                    CreatedAt= DateTime.UtcNow,
                    Currency= Logic.Enums.Currency.PLN,
                    Email = "test@example.com",
                    IsActive = true,
                    Gender = Logic.Enums.Gender.Female,
                    Address ="",
                    Age=99,
                    PasswordHash="123456",
                    Phone="",
                    Registered=DateTime.UtcNow,
                    TransactionId=_currentTransactionId,
                    Transactions = new List<Transaction> {},
                    Role = new Role () { Id=Guid.NewGuid().ToString(), CreatedAt = DateTime.UtcNow,  Name = "User"}
                };
        }

        private IQueryable<User> GetUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = _currentUserId,
                    FirstName= "Test",
                    LastName= "Test",
                    Company="",
                    Balance=4000m,
                    CreatedAt= DateTime.UtcNow,
                    Currency= Logic.Enums.Currency.PLN,
                    Email = "test@example.com",
                    IsActive = true,
                    Gender = Logic.Enums.Gender.Female,
                    Address ="",
                    Age=99,
                    PasswordHash="123456",
                    Phone="",
                    Registered=DateTime.UtcNow,
                    TransactionId=_currentTransactionId,
                    Transactions = new List<Transaction> {},
                    Role = new Role () { Id=Guid.NewGuid().ToString(), CreatedAt = DateTime.UtcNow,  Name = "User"}
                }
            }.AsQueryable();
        }

        private IQueryable<Transaction> GetTransactions()
        {
            var transactions = new List<Transaction>()
            {
                new Transaction ()
                {
                    Id=_currentTransactionId,
                    Amount=0,
                    BalanceAfterTransaction =0,
                    Date=DateTime.UtcNow,
                    Description="test",
                    Category = Logic.Enums.CategoryOfTransaction.Car,
                    Type = Logic.Enums.TypeOfTransaction.Outcome,
                    CreatedAt=DateTime.UtcNow,
                    Currency = Logic.Enums.Currency.PLN,
                    UserId=_currentUserId,
                    User = new User
                    {
                        Id = _currentUserId,
                        FirstName= "Test",
                        LastName= "Test",
                        Company="",
                        Balance=4000m,
                        CreatedAt= DateTime.UtcNow,
                        Currency= Logic.Enums.Currency.PLN,
                        Email = "test@example.com",
                        IsActive = true,
                        Gender = Logic.Enums.Gender.Female,
                        Address ="",
                        Age=99,
                        PasswordHash="123456",
                        Phone="",
                        Registered=DateTime.UtcNow,
                        TransactionId=_currentTransactionId,
                        Transactions = new List<Transaction> {},
                        Role = new Role () { Id=Guid.NewGuid().ToString(), CreatedAt = DateTime.UtcNow,  Name = "User"}
                    }
                }
            }.AsQueryable();
            return transactions;
        }
    }
}
