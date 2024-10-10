using FinancialPlanner.ConsoleApp;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Repository;
using FinancialPlanner.Logic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//var connectionString = "Server=localhost\\sqlexpress;Database=PlannerDb;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=false;";
var provider = "WinPg";
ConfigurationManager configuration = new ConfigurationManager();
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
configuration.AddEnvironmentVariables();

//-------------------------------------------------------
// -------------- ustalenie providera -------------------
//-------------------------------------------------------
if (environmentName == null)
{
    provider = EnumProvider.Default.ToString();
    throw new Exception("Select correct environment (WinPg lub LinuxPg)!!! brak bazy danych");
}
else if (environmentName == "LinuxPg")//TODO zmiana providera gdy wybrane spec. srodowisko
{
    provider = EnumProvider.LinuxPg.ToString();
}
else if (environmentName == "WinPg")//TODO zmiana providera gdy wybrane spec. srodowisko
{
    provider = EnumProvider.WinPg.ToString();
}

configuration.AddJsonFile($"appsettings.json", true, true);

configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "SqlProvider", provider },
        });

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);//TODO dodane aby poprawic blad zapisu czasu utc w postgres

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<ApplicationDbContext, PostgresDbContext>();
        services.AddTransient<IUserService<User>, UserService>();
        services.AddTransient<ITransactionService,TransactionService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ConfigurationManager>();
        services.AddSingleton<IConfiguration>(configuration);
    })
    .Build();

var svc = ActivatorUtilities.CreateInstance<AppStart>(host.Services);
svc.Run(args);