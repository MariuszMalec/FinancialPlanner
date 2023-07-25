using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinancialPlanner.Logic.Context
{
    public class PostgresDbContext : ApplicationDbContext
    {
        public PostgresDbContext(IConfiguration configuration)
            : base(configuration)
        {
        }

        public override DbSet<User> Users { get; set; }

        public override DbSet<Role> Roles { get; set; }

        public override DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var provider = Configuration["SqlProvider"];

            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            switch (provider)
            {
                case "WinPg":
                    options.UseNpgsql(Configuration.GetConnectionString(EnumProvider.WinPg.ToString()));
                    break;

                case "LinuxPg":
                    options.UseNpgsql(Configuration.GetConnectionString(EnumProvider.LinuxPg.ToString()));
                    break;
            }
            //options.UseNpgsql("Server = localhost; Port=5432; User Id=postgres; Password=Alicja13a; Database=PlannerDb;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
            base.OnModelCreating(modelBuilder);
        }
    }
}
