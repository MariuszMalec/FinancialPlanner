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

        public override DbSet<TransactionPicture> TransactionPictures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var provider = Configuration["ProviderFromAppsettings"];

            if (provider == null)
            {
                provider = "WinPg";//TODO aby zadzialala migracja to musi byc, nie czyta configuration!?
            }

            switch (provider)
            {
                case "WinPg":
                    options.UseNpgsql(Configuration.GetConnectionString(EnumProvider.WinPg.ToString()));
                    break;

                case "LinuxPg":
                    options.UseNpgsql(Configuration.GetConnectionString(EnumProvider.LinuxPg.ToString()));
                    break;
                default:
                    throw new ArgumentNullException(nameof(provider));
            }
            //options.UseNpgsql("Server = localhost; Port=5432; User Id=postgres; Password=mario13; Database=PlannerDb;");
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
