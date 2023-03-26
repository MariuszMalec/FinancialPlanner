using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Services;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FinancialPlanner.Logic.Context
{
    public static class SeedData
    {

        private static readonly List<User> _users = LoadDataService<User>.ReadUserFile();

        private static readonly List<Role> _roles = new List<Role>() 
        {
            new Role() { Id = Guid.NewGuid().ToString() , Name = "User" },
            new Role() { Id = Guid.NewGuid().ToString() , Name = "Admin" }
        };

        public static async Task SeedUsers(ApplicationDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            //foreach (var user in _users)
            //{
            //    user.Role = new Role() { 
            //        Id = context.Roles.AsNoTracking().Where(r=>r.Name == "User").First().Id,
            //        CreatedAt = context.Roles.AsNoTracking().Where(r => r.Name == "User").First().CreatedAt,
            //        Name = context.Roles.AsNoTracking().Where(r => r.Name == "User").First().Name 
            //    };
            //}

            var user = new User()
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
                }               
            };

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
    }
}
