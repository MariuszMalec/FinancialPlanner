using AutoMapper;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Repository;
using FinancialPlanner.WebMvc.Controllers;
using FinancialPlanner.WebMvc.Profiles;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;

namespace FinancialPlanner.XUnitIntegratedTests.UserControllerTests
{
    public class UserControllerTests
    {
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
        public void GetUserTransactions_ReturnFalse_WhenUserTransactionsNotExist()
        {
            //Arange
            var mockRepo = new Mock<IUserService>();
            mockRepo.Setup(r => r.GetAllQueryable().Result)
                .Returns(GetAllFake());

            var myProfile = new UserViewProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var controller = new UserController(mockRepo.Object, mapper);

            var id = GetAllFake().Select(x=>x.Id).FirstOrDefault();
            var userid = GetAllFake().Select(x => x.Id).FirstOrDefault();

            // Act
            var result = controller.GetUserTransactions(id, userid, "", "");

            // Assert
            //var viewResult = Assert.IsType<Task<ActionResult<List<UserDto>>>>(result);
            //var results = Assert.IsType<ViewResult>(viewResult.Result.Result);
            //var model = Assert.IsType<List<UserDto>>(results.Model);

            //Assert.Equal("Test", model.Select(x => x.FirstName).FirstOrDefault());
        }

        [Fact]
        public void Index_ReturnFalse_WhenUsersNotExist()
        {
            //Arange
            var mockRepo = new Mock<IUserService>();
            mockRepo.Setup(r => r.GetAllQueryable().Result)
                .Returns(GetAllFake());

            var myProfile = new UserViewProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var controller = new UserController(mockRepo.Object, mapper);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<Task<ActionResult<List<UserDto>>>>(result);
            var results = Assert.IsType<ViewResult>(viewResult.Result.Result);
            var model = Assert.IsType<List<UserDto>>(results.Model);

            Assert.Equal("Test", model.Select(x=>x.FirstName).FirstOrDefault());
        }

        private IQueryable<User> GetAllFake()
        {
            return new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid().ToString(),
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
                    TransactionId=Guid.NewGuid().ToString(),
                    Transactions = new List<Transaction> {},
                    Role = new Role () { Id=Guid.NewGuid().ToString(), CreatedAt = DateTime.UtcNow,  Name = "User"}
                }
            }.AsQueryable();
        }
    }
}
