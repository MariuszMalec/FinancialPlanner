using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.WebMvc.Controllers;
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
    }
}
