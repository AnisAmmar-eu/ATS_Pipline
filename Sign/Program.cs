using System.Configuration;
using Core.Configuration.Serilog;
using Core.Entities.Vision.ToDos.Services.ToSigns;
using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background.Vision.Signs;
using Core.Shared.Services.SystemApp.Logs;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Shared.DLLVision;
using Microsoft.EntityFrameworkCore;
using Core.Shared.Services.Background;
using Serilog;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options => options.ServiceName = "Sign service");

// Use Serilog as logger
builder.Logging.ClearProviders();
builder.Services.AddSerilog(
	(logger) => {
		logger
			.ReadFrom
			.Configuration(builder.Configuration)
			.Enrich
			.WithCustomEnrichers(builder.Configuration);
	});

builder.Services.AddDbContext<AnodeCTX>(
	options => options.UseSqlServer(builder.Configuration.GetConnectionStringWithThrow("DefaultConnection")));

builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IToSignService, ToSignService>();

builder.Services.AddSingleton<SignService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<SignService>());

builder.Services.AddSingleton<SignFileSettingService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<SignFileSettingService>());

builder.Services.AddSingleton<WatchDogServiceSign>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<WatchDogServiceSign>());

builder.Services.AddSingleton<ModeDebugService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<ModeDebugService>());

IHost host = builder.Build();

// Initialize
string dbInitialize = builder.Configuration["DbInitialize"]
	?? throw new ConfigurationErrorsException("Missing DbInitialize");
using IServiceScope scope = host.Services.CreateScope();
IServiceProvider services = scope.ServiceProvider;

if (bool.Parse(dbInitialize))
{
	AnodeCTX context = services.GetRequiredService<AnodeCTX>();
}

Microsoft.Extensions.Logging.ILogger logger = host.Services.GetRequiredService<ILogger<Program>>();

string dllPath = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.DLLPath);

string anodeType = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.AnodeType);
string stationName = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.StationName);
string folderParams = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.FolderParams);
string archivePath = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.ArchivePath);

string folderWithoutCam = Path.Combine(folderParams, stationName, anodeType);
string folderArchive = Path.Combine(archivePath, stationName, anodeType);

DLLVisionImport.SetDllDirectory(dllPath);
int retInit = DLLVisionImport.fcx_init();
string signStaticParams = Path.Combine(folderWithoutCam, ConfigDictionary.StaticSignName);

bool isNotArchiveStatic = File.Exists(signStaticParams);
int signParamsStaticOutput = isNotArchiveStatic ?
	DLLVisionImport.fcx_register_sign_params_static(0, signStaticParams) :
	DLLVisionImport.fcx_register_sign_params_static(0, Path.Combine(folderArchive, ConfigDictionary.StaticSignName));

if (isNotArchiveStatic)
	logger.LogWarning("Sign SignParamStatic {static}.", signParamsStaticOutput);
else
	logger.LogWarning("Archive: Sign SignParamStatic {static}.", signParamsStaticOutput);

foreach (int cameraID in new int[] { 1, 2 })
{
	string signDynamicPathRelative = Path.Combine(cameraID.ToString(), ConfigDictionary.DynamicSignName);
	string signDynamicParams = Path.Combine(folderWithoutCam, signDynamicPathRelative);

	bool isNotArchiveDynamic = File.Exists(signDynamicParams);
	int signParamsDynOutput = isNotArchiveDynamic ?
		DLLVisionImport.fcx_register_sign_params_dynamic(cameraID, signDynamicParams) :
		DLLVisionImport.fcx_register_sign_params_dynamic(cameraID, Path.Combine(folderArchive, signDynamicPathRelative));

	if (isNotArchiveDynamic)
	{
		logger.LogWarning(
			"Sign with SignDyn {id} {dynamic}.",
			cameraID,
			signParamsDynOutput);
	}
	else
	{
		logger.LogWarning(
			"Archive: Sign with SignDyn {id} {dynamic}.",
			cameraID,
			signParamsDynOutput);
	}
}

host.Run();