using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Middleware;
using FinancialPlanner.Logic.Repository;
using FinancialPlanner.Logic.Services;
using FinancialPlannerBlazorWeb.Components;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//dodane
IConfiguration Configuration;
Configuration = builder.Configuration;
ConfigurationManager configuration = builder.Configuration;//TODO add special data like name provider to configuration 
configuration.AddJsonFile($"appsettings.json", true, true);
if (!File.Exists("appsettings.json"))
{
    throw new Exception("BRAK! appsettings.json");
}
var logger = new LoggerConfiguration()
      .ReadFrom.Configuration(configuration)//czytanie z appsettings.json
      .CreateLogger();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ConfigurationManager>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);//TODO dodane aby poprawic blad zapisu czasu utc w postgres
builder.Services.AddDbContext<ApplicationDbContext, PostgresDbContext>();

builder.Services.AddSerilog(logger);//inject serilog

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

//dodane
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (dataContext.Database.IsRelational())
    {

    }
    else
    {
        throw new NotImplementedException();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
