using AutoMapper;
using Castle.Core.Logging;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Repository;
using FinancialPlanner.Logic.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanner.XUnitIntegratedTests.UserServiceTests
{
    public class UserServiceTests
    {
        private readonly IUserService _sut;
        private readonly Mock<IRepository<User>> _repoMock = new Mock<IRepository<User>>();
        private readonly Mock<ILogger<UserService>> _loggerMock = new Mock<ILogger<UserService>>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

        public UserServiceTests()
        {
            _sut = new UserService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllUsers_ShoudReturnUsers_WhenUsersExist()
        {
            // Arrange
            //var mock = new Mock<ApplicationDbContext>();
            //mock.Setup(x => x.Set<User>()).Returns(GetUsers());

            _repoMock.Setup(x => x.GetAll()).ReturnsAsync(GetUsers());

            // Act
            var users = await _sut.GetAll();

            // Assert
            users.Should().NotBeEmpty();
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
                Address = "Sadowa",
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
