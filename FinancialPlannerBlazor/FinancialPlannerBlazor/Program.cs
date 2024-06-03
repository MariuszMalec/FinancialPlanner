using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Repository;
using FinancialPlanner.Logic.Services;
using FinancialPlannerBlazor.Components;
using FinancialPlanner.Logic.ExtentionsMethod;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
		.AddInteractiveServerComponents()
		.AddInteractiveWebAssemblyComponents();


var config = builder.Configuration;
// Add Entity Framework
builder.Services.AddEntityFramework(config);//add connection string

builder.Services.AddDbContext<ApplicationDbContext, PostgresDbContext>();

builder.Services.AddBlazorBootstrap();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
		.AddInteractiveServerRenderMode()
		.AddInteractiveWebAssemblyRenderMode()
		.AddAdditionalAssemblies(typeof(FinancialPlannerBlazor.Client._Imports).Assembly);

app.Run();
