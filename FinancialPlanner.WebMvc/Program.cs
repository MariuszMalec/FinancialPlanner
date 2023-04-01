using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Repository;
using FinancialPlanner.Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System;
using FinancialPlanner.WebMvc.Middleware;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//dodane
//builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseInMemoryDatabase("usersDb"));
IConfiguration Configuration;
Configuration = builder.Configuration;
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(Configuration.GetConnectionString("Default"));
    //options.UseNpgsql(Configuration.GetConnectionString("Linux"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);//TODO dodane aby poprawic blad zapisu czasu utc w postgres


builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

//dodane
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await SeedData.SeedRoles(dataContext);
    await SeedData.SeedUsers(dataContext);
    await SeedData.SeedTransaction(dataContext);
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
cultureInfo.NumberFormat.CurrencySymbol = "PLN";
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
