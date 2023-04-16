using FluentAssertions;

namespace FinancialPlanner.XUnitIntegratedTests.UserControllerTests
{
    public class UserContextTests : IClassFixture<UserSeedDataFixture>
    {
        UserSeedDataFixture _fixture;

        public UserContextTests(UserSeedDataFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GetUsers_ShoudReturnUsers_WhenUsersExist()
        {
            // Arrange
            var users = _fixture.UserContext.Users;

            // Act
            var result = users.Count();

            // Assert
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public void GetUser_ShoudReturnUser_WhenUserHasEmailAsAdmin()
        {
            // Arrange
            var users = _fixture.UserContext.Users;

            // Act
            var result = users.FirstOrDefault();

            // Assert
            Assert.Equal("Admin@example.com", result.Email);
        }
    }
}
