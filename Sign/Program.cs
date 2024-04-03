using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Services.Background.Vision;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Shared.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Core.Shared.Services.Background;
using Core.Entities.Vision.ToDos.Services.ToSigns;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Microsoft.AspNetCore.Identity;
using System.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background.KPI.KPILogs;
using Core.Shared.Services.SystemApp.Logs;
using DLLVision;
using Microsoft.Extensions.Configuration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
	options.ServiceName = "Sign service";
});


builder.Configuration.LoadBaseConfiguration();

builder.Services.AddDbContext<AnodeCTX>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionStringWithThrow("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AnodeCTX>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IToSignService, ToSignService>();


builder.Services.AddSingleton<SignService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<SignService>());

var host = builder.Build();

// Initialize
string? dbInitialize = builder.Configuration["DbInitialize"];
if (dbInitialize is null)
throw new ConfigurationErrorsException("Missing DbInitialize");
if (!Station.IsServer && bool.Parse(dbInitialize))
{
    using IServiceScope scope = host.Services.CreateScope();
    IServiceProvider services = scope.ServiceProvider;
    AnodeCTX context = services.GetRequiredService<AnodeCTX>();
    UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
}

string DLLPath = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.DLLPath);
DLLVisionImport.SetDllDirectory(DLLPath);
int retInit = DLLVisionImport.fcx_init();

host.Run();
