using FinancialPlanner.Logic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinancialPlanner.Logic.Context
{
    public abstract class ApplicationDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public abstract DbSet<User> Users { get; set; }

        public abstract DbSet<Role> Roles { get; set; }

        public abstract DbSet<Transaction> Transactions { get; set; }

        protected ApplicationDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
