using FinancialPlanner.Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialPlanner.Logic.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase("usersDb");//TODO to samo w appsettings.json jest
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().ToTable("Users");
            //base.OnModelCreating(modelBuilder);
        }
    }
}
