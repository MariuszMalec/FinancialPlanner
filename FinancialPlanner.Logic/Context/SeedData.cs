using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Services;
using System.Data;

namespace FinancialPlanner.Logic.Context
{
    public static class SeedData
    {

        //private static readonly List<User> _users = LoadDataService<User>.ReadUserFile();

        //private static readonly List<Transaction> _transactions = LoadDataService<Transaction>.ReadUserFile();

        private static readonly List<Role> _roles = new List<Role>() 
        {
            new Role() { Id = Guid.NewGuid().ToString() , Name = "User" },
            new Role() { Id = Guid.NewGuid().ToString() , Name = "Admin" }
        };

        private static readonly List<User> _users = new List<User>()
        {
            new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Mariusz",
                LastName = "Malec",
                Age = 47,
                Balance = 3000,
                Address = "Sadowa",
                Company = "GE",
                Currency = Enums.Currency.PLN,
                Gender = Enums.Gender.Male,
                Email = "mario@example.com",
                IsActive = true,
                Phone = "",
                Registered = DateTime.Now,
                Role = new Role()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    Name = "SuperAdmin"
                },
                TransactionId= _roles.Select(x => x.Id).First()
            },
        };

        private static readonly List<Transaction> _transactions = new List<Transaction>()
        {
            new Transaction() { 
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                Amount= 0,
                BalanceAfterTransaction= 0,
                Category = Enums.CategoryOfTransaction.Salary,
                Type = Enums.TypeOfTransaction.Income,
                Currency = Enums.Currency.PLN,
                Description = "W morde kredyt",
                CreatedAt= DateTime.Now,
                UserId = _users.Select(x => x.Id).First()
            },
        };

        public static async Task SeedUsers(ApplicationDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var user = _users.First();

            var encodePassword = Base64EncodeDecode.Base64Encode("123456");
            user.PasswordHash = encodePassword;

            await context.AddAsync(user);

            //TODO jak dodac liste z rola => blad save bo probuje dodac id do rol gdzie juz rola jest !!!????
            //await context.Users.AddRangeAsync(_users);
            await context.SaveChangesAsync();
        }

        public static async Task SeedRoles(ApplicationDbContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }
            await context.Roles.AddRangeAsync(_roles);
            await context.SaveChangesAsync();
        }

        public static async Task SeedTransaction(ApplicationDbContext context)
        {
            if (context.Transactions.Any())
            {
                return;
            }
            await context.Transactions.AddRangeAsync(_transactions);
            await context.SaveChangesAsync();
        }
    }
}
