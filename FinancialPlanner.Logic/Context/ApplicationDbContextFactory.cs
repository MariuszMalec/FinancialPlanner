using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FinancialPlanner.Logic.Context
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress;Database=PlannerDb;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=false;");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
