using ElevatorSystem.DataLayer;
using ElevatorSystem.BusinessLayer;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IElevatorDL, ElevatorDL>();
builder.Services.AddTransient<IElevatorBL, ElevatorBL>();   
builder.Services.AddHostedService<SimulationService>();

builder.Services.AddSignalR();

Log.Logger = new LoggerConfiguration()
       .ReadFrom.Configuration(builder.Configuration)
       .Enrich.FromLogContext()
       .WriteTo.Console()
       .WriteTo.File("Logs/ElevatorSystem.txt", rollingInterval: RollingInterval.Day)
       .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ElevatorHub>("/elevatorHub");

app.Run();
