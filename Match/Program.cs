using Core.Entities.User.Models.DB.Users;
using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background.Vision;
using DLLVision;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "Match service";
});


builder.Configuration.LoadBaseConfiguration();

builder.Services.AddDbContext<AnodeCTX>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionStringWithThrow("DefaultConnection")));

builder.Services.AddSingleton<MatchService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<MatchService>());

builder.Services.AddSingleton<MatchFileSettingService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<MatchFileSettingService>());

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
string matchDynamicParams = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.MatchDynParams);

DLLVisionImport.SetDllDirectory(DLLPath);
int retInit = DLLVisionImport.fcx_init();
int signParamsStaticOutput = DLLVisionImport.fcx_register_sign_params_static(0, signStaticParams);
int matchParamsDynOutput = DLLVisionImport.fcx_register_match_params_dynamic(0, matchDynamicParams);

host.Run();
