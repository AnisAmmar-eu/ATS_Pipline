using Core.Entities.User.Models.DB.Users;
using Core.Entities.Vision.ToDos.Services.ToMatchs;
using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background.Vision.Matchs;
using Core.Shared.Services.SystemApp.Logs;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Shared.UnitOfWork;
using DLLVision;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Core.Entities.User.Models.DB.Roles;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options => options.ServiceName = "Match service");

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

builder.Services.AddSingleton<WatchDogServiceMatch>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<WatchDogServiceMatch>());

IHost host = builder.Build();

// Initialize
string? dbInitialize = builder.Configuration["DbInitialize"]
	?? throw new ConfigurationErrorsException("Missing DbInitialize");
using IServiceScope scope = host.Services.CreateScope();
IServiceProvider services = scope.ServiceProvider;

if (bool.Parse(dbInitialize))
{
	AnodeCTX context = services.GetRequiredService<AnodeCTX>();
	UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
}

ILogger logger = host.Services.GetRequiredService<ILogger<Program>>();

List<int> GPUID = builder.Configuration.GetSectionWithThrow<List<int>>(ConfigDictionary.GPUID);

string DLLPath = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.DLLPath);

// FolderPath = FolderParams//InstanceMatchID//CameraID
string instanceMatchID = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.InstanceMatchID);
string folderParams = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.FolderParams);

string folderWithoutCam = Path.Combine(folderParams, instanceMatchID);

DLLVisionImport.SetDllDirectory(DLLPath);
int retInit = DLLVisionImport.fcx_init();
string signStaticParams = Path.Combine(folderWithoutCam, ConfigDictionary.StaticSignName);
int signParamsStaticOutput = DLLVisionImport.fcx_register_sign_params_static(0, signStaticParams);

logger.LogInformation("Match SignParamStatic {static}.", signParamsStaticOutput);

foreach (int cameraID in new int[] {1, 2})
{
	string folderPath = Path.Combine(folderWithoutCam, cameraID.ToString());
	string matchDynamicParams = Path.Combine(folderPath, ConfigDictionary.DynamicMatchName);

	int matchParamsDynOutput = DLLVisionImport.fcx_register_match_params_dynamic(cameraID, matchDynamicParams);
	int registerDatasetOutput = DLLVisionImport.fcx_register_dataset(cameraID, 0, GPUID[cameraID-1]);

	logger.LogInformation(
		"Match with matchDyn {id} {matchDyn} and Dataset {id} {dataset}.",
		cameraID,
		matchParamsDynOutput,
		cameraID,
		registerDatasetOutput);
}

host.Run();