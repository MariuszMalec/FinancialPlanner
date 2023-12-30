using FinancialPlanner.Logic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinancialPlanner.Logic.Context
{
    public class MemoryDbContext : ApplicationDbContext
    {
        public MemoryDbContext(IConfiguration configuration)
            : base(configuration)
        {
        }

        public override DbSet<User> Users { get; set; }

        public override DbSet<Role> Roles { get; set; }

        public override DbSet<Transaction> Transactions { get; set; }

        public override DbSet<TransactionPicture> TransactionPictures { get; set; }
        public override DbSet<CategoryBudget> CategoryBudgets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("FinancialPlannerDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
            modelBuilder.Entity<TransactionPicture>().ToTable("TransactionPictures");
            base.OnModelCreating(modelBuilder);
        }
    }
}
