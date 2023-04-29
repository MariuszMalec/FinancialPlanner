using FinancialPlanner.ConsoleApp;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Repository;
using FinancialPlanner.Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var connectionString = "Server=localhost\\sqlexpress;Database=PlannerDb;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=false;";

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            //options.UseNpgsql(Configuration.GetConnectionString("Linux"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITransactionService,TransactionService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    })
    .Build();

var svc = ActivatorUtilities.CreateInstance<AppStart>(host.Services);
svc.Run(args);

//MainMenu.ShowMainMenu();