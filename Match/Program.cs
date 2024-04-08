using Core.Entities.User.Models.DB.Users;
using Core.Entities.Vision.ToDos.Services.ToMatchs;
using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background.Vision;
using Core.Shared.Services.SystemApp.Logs;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Shared.UnitOfWork;
using DLLVision;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Core.Entities.User.Models.DB.Roles;
using System.Data;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
	options.ServiceName = "Match service";
});


builder.Configuration.LoadBaseConfiguration();

builder.Services.AddDbContext<AnodeCTX>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionStringWithThrow("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
	.AddEntityFrameworkStores<AnodeCTX>()
	.AddDefaultTokenProviders();

builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IToMatchService, ToMatchService>();

builder.Services.AddSingleton<LoadService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<LoadService>());

builder.Services.AddSingleton<MatchService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<MatchService>());

builder.Services.AddSingleton<UnloadService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<UnloadService>());

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
List<string> datasets = builder.Configuration.GetSectionWithThrow<List<string>>(ConfigDictionary.Datasets);
List<int> GPUID = builder.Configuration.GetSectionWithThrow<List<int>>(ConfigDictionary.GPUID);

DLLVisionImport.SetDllDirectory(DLLPath);
int retInit = DLLVisionImport.fcx_init();

for (int i = 0; i < datasets.Count; i++)
{
	if (datasets[i] == string.Empty)
		continue;

	int signParamsStaticOutput = DLLVisionImport.fcx_register_sign_params_static(i, Path.Combine(datasets[i], signStaticParams));
	int matchParamsDynOutput = DLLVisionImport.fcx_register_match_params_dynamic(i, Path.Combine(datasets[i], matchDynamicParams));
	int registerDatasetOutput = DLLVisionImport.fcx_register_dataset(i, i, GPUID[i]);
}


host.Run();
