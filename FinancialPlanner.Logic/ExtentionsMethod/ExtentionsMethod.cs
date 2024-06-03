using FinancialPlanner.Logic.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialPlanner.Logic.ExtentionsMethod
{
    public static class ExtentionsMethod
    {
        public static bool IsAjaxRequest(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return !string.IsNullOrEmpty(request.Headers["X-Requested-With"]) &&
                    string.Equals(
                        request.Headers["X-Requested-With"],
                        "XmlHttpRequest",
                        StringComparison.OrdinalIgnoreCase);

            return false;
        }

        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("WinPg");
            services.AddDbContext<PostgresDbContext>(options => options.UseNpgsql(connectionString));
            return services;
        }
    }
}
