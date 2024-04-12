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

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options => options.ServiceName = "Sign service");

builder.Configuration.LoadBaseConfiguration();

builder.Services.AddDbContext<AnodeCTX>(
	options => options.UseSqlServer(builder.Configuration.GetConnectionStringWithThrow("DefaultConnection")));

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

IHost host = builder.Build();

// Initialize
string dbInitialize = builder.Configuration["DbInitialize"]
	?? throw new ConfigurationErrorsException("Missing DbInitialize");

if (bool.Parse(dbInitialize))
{
	using IServiceScope scope = host.Services.CreateScope();
	IServiceProvider services = scope.ServiceProvider;
	AnodeCTX context = services.GetRequiredService<AnodeCTX>();
	UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
}

string DLLPath = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.DLLPath);

// FolderPath = FolderParams//StationName//AnodeType//CameraID
string anodeType = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.AnodeType);
string stationName = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.StationName);
string folderParams = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.FolderParams);

string folderWithoutCam = Path.Combine(folderParams, stationName, anodeType);

DLLVisionImport.SetDllDirectory(DLLPath);
int retInit = DLLVisionImport.fcx_init();
string signStaticParams = Path.Combine(folderWithoutCam, ConfigDictionary.StaticSignName);

int signParamsStaticOutput = DLLVisionImport.fcx_register_sign_params_static(0, signStaticParams);

foreach (int cameraID in new int[] {1, 2})
{
	string folderPath = Path.Combine(folderWithoutCam, cameraID.ToString());
	string signDynamicParams = Path.Combine(folderPath, ConfigDictionary.DynamicSignName);

	int signParamsDynOutput = DLLVisionImport.fcx_register_sign_params_dynamic(cameraID, signDynamicParams);
}

host.Run();