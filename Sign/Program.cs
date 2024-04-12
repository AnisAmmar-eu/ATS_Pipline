using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Services.Background.Vision;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Shared.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Core.Shared.Services.SystemApp.Logs;
using Core.Entities.Vision.ToDos.Services.ToSigns;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Microsoft.AspNetCore.Identity;
using System.Configuration;
using Core.Shared.Dictionaries;
using DLLVision;

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

builder.Services.AddSingleton<SignFileSettingService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<SignFileSettingService>());

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
string signStaticParams = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.SignStaticParams);
string signDynamicParams = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.SignDynParams);

DLLVisionImport.SetDllDirectory(DLLPath);
int retInit = DLLVisionImport.fcx_init();

int signParamsStaticOutput = DLLVisionImport.fcx_register_sign_params_static(0, signStaticParams);
int signParamsDynOutput = DLLVisionImport.fcx_register_sign_params_dynamic(0, signDynamicParams);

host.Run();
