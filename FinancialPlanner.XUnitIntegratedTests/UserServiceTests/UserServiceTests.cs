using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Repository;
using FinancialPlanner.Logic.Services;
using FinancialPlanner.XUnitIntegratedTests.UserControllerTests;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FinancialPlanner.XUnitIntegratedTests.UserServiceTests
{
    public class UserServiceTests : IClassFixture<UserSeedDataFixture>
    {
        private readonly IUserService<User> _sut;
        private readonly Mock<IRepository<User>> _repoMock = new Mock<IRepository<User>>();
        private readonly Mock<ILogger<UserService>> _loggerMock = new Mock<ILogger<UserService>>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        UserSeedDataFixture _fixture;

        public UserServiceTests(UserSeedDataFixture fixture = null)
        {
            _sut = new UserService(_repoMock.Object, fixture.UserContext , _loggerMock.Object, _mapperMock.Object);
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllUsers_ShoudReturnUsers_WhenUsersExist()
        {
            // Arrange
            _repoMock.Setup(x => x.GetAll()).ReturnsAsync(GetUsers());

            // Act
            var users = await _sut.GetAll();

            // Assert
            users.Should().NotBeEmpty();
            users.Should().HaveCount(1);
        }

        private IEnumerable<User> GetUsers()
        {
            var sessions = new List<User>();
            sessions.Add(new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Mariusz",
                LastName = "Malec",
                Age = 47,
                Balance = 3000,
                Address = "Sadowa 1",
                Company = "GE",
                Currency = Logic.Enums.Currency.PLN,
                Gender = Logic.Enums.Gender.Male,
                Email = "mario@example.com",
                IsActive = true,
                Phone = "",
                PasswordHash= "123456",
                Registered = DateTime.Now,
                Role = new Role()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    Name = "SuperAdmin"
                },
                TransactionId = Guid.NewGuid().ToString(),
                Transactions= new List<Transaction>() { },
                CreatedAt= DateTime.Now
            });
            return sessions;
        }
    }
}
