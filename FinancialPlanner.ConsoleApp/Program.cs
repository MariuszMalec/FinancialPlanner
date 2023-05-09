using FinancialPlanner.ConsoleApp;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Repository;
using FinancialPlanner.Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var connectionString = "Server=localhost\\sqlexpress;Database=PlannerDb;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=false;";
var provider = "WinPg";
ConfigurationManager configuration = new ConfigurationManager();
configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "SqlProvider", provider },
        });
var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<ApplicationDbContext, PostgresDbContext>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITransactionService,TransactionService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ConfigurationManager>();
    })
    .Build();
var svc = ActivatorUtilities.CreateInstance<AppStart>(host.Services);
svc.Run(args);