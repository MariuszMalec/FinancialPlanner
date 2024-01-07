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
                Balance = 0,
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
                Amount= 6700,
                BalanceAfterTransaction= 6700,
                Category = Enums.CategoryOfTransaction.Salary,
                Type = Enums.TypeOfTransaction.Income,
                Currency = Enums.Currency.PLN,
                Description = "wyplata",
                CreatedAt= DateTime.Now,
                UserId = _users.Select(x => x.Id).First(),
                User = new User() { IsActive = true,
                    Balance = 6700,
                    LastName = _users.Select(x => x.LastName).First(),
                    FirstName = _users.Select(x => x.FirstName).First(),
                    Email = _users.Select(x => x.Email).First(),
                    Age = _users.Select(x => x.Age).First(),
                    Role = _users.Select(x => x.Role).First(),
                    }
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

        public static async Task SeedTransactionPictures(ApplicationDbContext context)
        {
            if (context.TransactionPictures.Any())
            {
                return;
            }
            await context.TransactionPictures.AddRangeAsync(_transactionPictures);
            await context.SaveChangesAsync();
        }

        public static async Task SeedCategoryBudget(ApplicationDbContext context)
        {
            if (context.CategoryBudgets.Any())
            {
                return;
            }
            var _categoryBudgets = GetCategoryBudgets(context);
            await context.CategoryBudgets.AddRangeAsync(_categoryBudgets);
            await context.SaveChangesAsync();
        }

        private static List<CategoryBudget> GetCategoryBudgets(ApplicationDbContext context)
        {
            var userId = context.Users.OrderByDescending(u=>u.Id).First().Id;
            List<CategoryBudget> _categoryBudgets = new List<CategoryBudget>()
            {
                new CategoryBudget() { Id = Guid.NewGuid().ToString(),
                    Category = Enums.CategoryOfTransaction.Food,
                    PlanedBudget = 2500,
                    CreatedAt = DateTime.Now,
                    UserId = userId},
                new CategoryBudget() { Id = Guid.NewGuid().ToString(),
                    Category = Enums.CategoryOfTransaction.Home,
                    PlanedBudget = 800,
                    CreatedAt = DateTime.Now,
                    UserId = userId},
                new CategoryBudget() { Id = Guid.NewGuid().ToString(),
                    Category = Enums.CategoryOfTransaction.Car,
                    PlanedBudget = 300,
                    CreatedAt = DateTime.Now,
                    UserId = userId},
                new CategoryBudget() { Id = Guid.NewGuid().ToString(),
                    Category = Enums.CategoryOfTransaction.Entertainment,
                    PlanedBudget = 250,
                    CreatedAt = DateTime.Now,
                    UserId = userId},
                new CategoryBudget() { Id = Guid.NewGuid().ToString(),
                    Category = Enums.CategoryOfTransaction.Medicine,
                    PlanedBudget = 150,
                    CreatedAt = DateTime.Now,
                    UserId = userId},
                new CategoryBudget() { Id = Guid.NewGuid().ToString(),
                    Category = Enums.CategoryOfTransaction.Credit,
                    PlanedBudget = 1700,
                    CreatedAt = DateTime.Now,
                    UserId = userId},
                new CategoryBudget() { Id = Guid.NewGuid().ToString(),
                    Category = Enums.CategoryOfTransaction.Kids,
                    PlanedBudget = 250,
                    CreatedAt = DateTime.Now,
                    UserId = userId},
                new CategoryBudget() { Id = Guid.NewGuid().ToString(),
                    Category = Enums.CategoryOfTransaction.School,
                    PlanedBudget = 500,
                    CreatedAt = DateTime.Now,
                    UserId = userId},
                new CategoryBudget() { Id = Guid.NewGuid().ToString(),
                    Category = Enums.CategoryOfTransaction.EatingOut,
                    PlanedBudget = 100,
                    CreatedAt = DateTime.Now,
                    UserId = userId},
                new CategoryBudget() { Id = Guid.NewGuid().ToString(),
                    Category = Enums.CategoryOfTransaction.Salary,
                    PlanedBudget = 6500,
                    CreatedAt = DateTime.Now,
                    UserId = userId},
                new CategoryBudget() { Id = Guid.NewGuid().ToString(),
                    Category = Enums.CategoryOfTransaction.Clothing,
                    PlanedBudget = 250,
                    CreatedAt = DateTime.Now,
                    UserId = userId},
                new CategoryBudget() { Id = Guid.NewGuid().ToString(),
                    Category = Enums.CategoryOfTransaction.Commute,
                    PlanedBudget = 100,
                    CreatedAt = DateTime.Now,
                    UserId = userId},
           };
            return _categoryBudgets;
        }

        private static readonly List<TransactionPicture> _transactionPictures = new List<TransactionPicture>()
        {
            new TransactionPicture() {
                Id = Guid.NewGuid().ToString(),
                Category = Enums.CategoryOfTransaction.Home,
                Source = "https://images.unsplash.com/photo-1501183638710-841dd1904471?auto=format&fit=crop&q=60&w=500&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8aG9tZXxlbnwwfHwwfHx8MA%3D%3D"
            },
            new TransactionPicture() {
                Id = Guid.NewGuid().ToString(),
                Category = Enums.CategoryOfTransaction.Car,
                Source = "https://images.unsplash.com/photo-1502877338535-766e1452684a?auto=format&fit=crop&q=60&w=500&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8Y2FyfGVufDB8fDB8fHww"
            },
            new TransactionPicture() {
                Id = Guid.NewGuid().ToString(),
                Category = Enums.CategoryOfTransaction.Clothing,
                Source = "https://images.unsplash.com/photo-1560243563-062bfc001d68?auto=format&fit=crop&q=60&w=500&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8OHx8Y2xvdGhpbmd8ZW58MHx8MHx8fDA%3D"
            },
            new TransactionPicture() {
                Id = Guid.NewGuid().ToString(),
                Category = Enums.CategoryOfTransaction.School,
                Source = "https://images.unsplash.com/photo-1580582932707-520aed937b7b?auto=format&fit=crop&q=60&w=500&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8M3x8c2Nob29sfGVufDB8fDB8fHww"
            },
            new TransactionPicture() {
                Id = Guid.NewGuid().ToString(),
                Category = Enums.CategoryOfTransaction.Food,
                Source = "https://images.unsplash.com/photo-1557821552-17105176677c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1332&q=80"
            },
            new TransactionPicture() {
                Id = Guid.NewGuid().ToString(),
                Category = Enums.CategoryOfTransaction.Salary,
                Source = "https://images.unsplash.com/photo-1553729459-efe14ef6055d?auto=format&fit=crop&q=80&w=2070&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
            },
            new TransactionPicture() {
                Id = Guid.NewGuid().ToString(),
                Category = Enums.CategoryOfTransaction.Medicine,
                Source = "https://images.unsplash.com/photo-1584308666744-24d5c474f2ae?auto=format&fit=crop&q=80&w=2030&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
            },
            new TransactionPicture() {
                Id = Guid.NewGuid().ToString(),
                Category = Enums.CategoryOfTransaction.Credit,
                Source = "https://images.unsplash.com/photo-1624811532681-e58a7e25f273?auto=format&fit=crop&q=80&w=2070&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
            },
            new TransactionPicture() {
                Id = Guid.NewGuid().ToString(),
                Category = Enums.CategoryOfTransaction.Entertainment,
                Source = "https://images.unsplash.com/photo-1567593810070-7a3d471af022?q=80&w=2071&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
            },
            new TransactionPicture() {
                Id = Guid.NewGuid().ToString(),
                Category = Enums.CategoryOfTransaction.Kids,
                Source = "https://plus.unsplash.com/premium_photo-1684623605109-263925d88106?q=80&w=1932&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
            },
            new TransactionPicture() {
                Id = Guid.NewGuid().ToString(),
                Category = Enums.CategoryOfTransaction.EatingOut,
                Source = "https://plus.unsplash.com/premium_photo-1664702602930-0755001218d6?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
            },
            new TransactionPicture() {
                Id = Guid.NewGuid().ToString(),
                Category = Enums.CategoryOfTransaction.ExtraMoney,
                Source = "https://images.unsplash.com/photo-1593672755342-741a7f868732?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
            },
            new TransactionPicture() {
                Id = Guid.NewGuid().ToString(),
                Category = Enums.CategoryOfTransaction.Special,
                Source = "https://images.unsplash.com/photo-1413708617479-50918bc877eb?q=80&w=2073&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
            },
        };
    }
}
