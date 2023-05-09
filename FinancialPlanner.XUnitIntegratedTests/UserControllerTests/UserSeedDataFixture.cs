using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinancialPlanner.XUnitIntegratedTests.UserControllerTests
{
    public class UserSeedDataFixture : IDisposable
    {
        public ApplicationDbContext UserContext { get; private set; }

        public UserSeedDataFixture()
        {
            //var options = new DbContextOptionsBuilder<MemoryDbContext>()
            //    .UseInMemoryDatabase("UsersDb")
            //    .Options;//TODO jak akywowac to tutaj

            ConfigurationManager configuration = new ConfigurationManager();

            UserContext = new MemoryDbContext(configuration);

            UserContext.Users.Add(new User
            {
                Address = "Adminowo",
                Email = "Admin@example.com",
                FirstName = "Admin",
                LastName = "Admin",
                Age= 30,
                CreatedAt= DateTime.Now,
                IsActive= true,
                Balance= 0,
                Company="",
                Currency = Logic.Enums.Currency.PLN,
                Gender = Logic.Enums.Gender.Male,
                Id = Guid.NewGuid().ToString(),
                PasswordHash = "",
                Phone ="",
                Registered=DateTime.Now,
                Role = new Role () { Name = "User"}
            });
            UserContext.SaveChanges();
        }

        public void Dispose()
        {
            UserContext.Database.EnsureDeleted();
            UserContext.Dispose();
        }
    }
}
