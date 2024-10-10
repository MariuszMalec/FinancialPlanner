using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Repository;
using FinancialPlanner.Logic.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

builder.Services.AddTransient<IUserService<User>, UserService>();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ConfigurationManager>();
//builder.Services.AddScoped<ErrorHandlingMiddleware>();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);//TODO dodane aby poprawic blad zapisu czasu utc w postgres
builder.Services.AddDbContext<ApplicationDbContext, PostgresDbContext>();

builder.Services.AddSerilog(logger);//inject serilog

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
