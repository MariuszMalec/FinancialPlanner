using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Repository;
using FinancialPlanner.Logic.Services;
using FinancialPlanner.WebMvc.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Globalization;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//dodane
IConfiguration Configuration;
Configuration = builder.Configuration;

ConfigurationManager configuration = builder.Configuration;//TODO add special data like name provider to configuration 

configuration.AddJsonFile($"appsettings.json", true, true);

if (!File.Exists("appsettings.json"))
{
    throw new Exception("BRAK! appsettings.json");
}

IWebHostEnvironment environment = builder.Environment;

var logger = new LoggerConfiguration()
      .ReadFrom.Configuration(configuration)//czytanie z appsettings.json
      .CreateLogger();
builder.Host.UseSerilog(logger);

logger.Information("Starting app ...");

//-------------------------------------------------------
// -------------- ustalenie providera -------------------
//-------------------------------------------------------
var provider = Configuration["ConnectionStrings"];//TODO nie czyta appsetings.json?? dodac provider domyslny w pliku 
if (environment.EnvironmentName == "Default" || environment.EnvironmentName == "Development")//TODO zmiana providera gdy wybrane spec. srodowisko
{
    provider = EnumProvider.Default.ToString();
}
else if (environment.EnvironmentName == "LinuxPg")//TODO zmiana providera gdy wybrane spec. srodowisko
{
    provider = EnumProvider.LinuxPg.ToString();
}
else if (environment.EnvironmentName == "WinPg")//TODO zmiana providera gdy wybrane spec. srodowisko
{
    provider = EnumProvider.WinPg.ToString();
}
else
{
    provider = EnumProvider.LinuxPg.ToString();
}

//TODO add static values which I can use f.e in homecontroller!
configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "SqlProvider", provider },
        });

switch (provider)
{
    case "Default":
        builder.Services.AddDbContext<ApplicationDbContext, MsqlDbContext>();
        break;

    case "WinPg":
        builder.Services.AddDbContext<ApplicationDbContext, PostgresDbContext>();
        break;

    case "LinuxPg":
        builder.Services.AddDbContext<ApplicationDbContext, PostgresDbContext>();
        break;
    default:
        throw new Exception($"Unsupported provider: {provider}");
}

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);//TODO dodane aby poprawic blad zapisu czasu utc w postgres

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITransactionService,TransactionService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<ConfigurationManager>();
builder.Services.AddSerilog(logger);//inject serilog

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

//dodane
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();//TODO tutaj wchodzi do OnConfiguring
    var takeConfiguration = scope.ServiceProvider.GetRequiredService<ConfigurationManager>();
    if (dataContext.Database.IsRelational())
    {
        if (dataContext.Database.IsRelational())
        {
            dataContext?.Database.Migrate();
            await SeedData.SeedRoles(dataContext);
            await SeedData.SeedUsers(dataContext);
            await SeedData.SeedTransaction(dataContext);
        }
    }
    else
    {
        //TODO nie ralacyjna baza danych np memory msql do testow
        await SeedData.SeedRoles(dataContext);
        await SeedData.SeedUsers(dataContext);
        await SeedData.SeedTransaction(dataContext);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Culture specific problems
var cultureInfo = new CultureInfo("en-GB");
cultureInfo.NumberFormat.CurrencySymbol = "PLN ";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program { }