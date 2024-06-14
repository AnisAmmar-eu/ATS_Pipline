using System.Configuration;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.Vision.ToDos.Services.ToMatchs;
using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background.Vision.Matchs;
using Core.Shared.Services.SystemApp.Logs;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Shared.DLLVision;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Vision.ToDos.Models.DB.Datasets;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Shared.Services.Background;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options => options.ServiceName = "Match service");

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

builder.Services.AddSingleton<ModeDebugService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<ModeDebugService>());

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

List<int> gpuID = builder.Configuration.GetSectionWithThrow<List<int>>(ConfigDictionary.GPUID);

string dllPath = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.DLLPath);

// FolderPath = FolderParams//InstanceMatchID//CameraID
string instanceMatchID = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.InstanceMatchID);
string folderParams = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.FolderParams);
string archivePath = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.ArchivePath);

string folderWithoutCam = Path.Combine(folderParams, instanceMatchID);
string folderArchive = Path.Combine(archivePath, instanceMatchID);

DLLVisionImport.SetDllDirectory(dllPath);
int retInit = DLLVisionImport.fcx_init();
string signStaticParams = Path.Combine(folderWithoutCam, ConfigDictionary.StaticSignName);

bool isNotArchiveStatic = File.Exists(signStaticParams);
int signParamsStaticOutput = isNotArchiveStatic ?
	DLLVisionImport.fcx_register_sign_params_static(0, signStaticParams) :
	DLLVisionImport.fcx_register_sign_params_static(0, Path.Combine(folderArchive, Path.GetFileName(signStaticParams)));

if (isNotArchiveStatic)
	logger.LogWarning("Match SignParamStatic {static}.", signParamsStaticOutput);
else
	logger.LogWarning("Archive: Match SignParamStatic {static}.", signParamsStaticOutput);

foreach (int cameraID in new int[] { 1, 2 })
{
	string matchDynamicPathRelative = Path.Combine(cameraID.ToString(), ConfigDictionary.DynamicMatchName);
	string matchDynamicParams = Path.Combine(folderWithoutCam, matchDynamicPathRelative);

	bool isNotArchive = File.Exists(matchDynamicParams);
	int matchParamsDynOutput = isNotArchive ?
		DLLVisionImport.fcx_register_match_params_dynamic(cameraID, matchDynamicParams) :
		DLLVisionImport.fcx_register_match_params_dynamic(cameraID, Path.Combine(folderArchive, Path.Combine(folderArchive, matchDynamicPathRelative)));
	int registerDatasetOutput = DLLVisionImport.fcx_register_dataset(cameraID, 0, gpuID[cameraID - 1]);

	if (isNotArchive)
	{
		logger.LogWarning(
			"Match with matchDyn {id} {matchDyn} and Dataset {id} {dataset}.",
			cameraID,
			matchParamsDynOutput,
			cameraID,
			registerDatasetOutput);
	}
	else
	{
		logger.LogWarning(
			"Archive: Match with matchDyn {id} {matchDyn} and Dataset {id} {dataset}.",
			cameraID,
			matchParamsDynOutput,
			cameraID,
			registerDatasetOutput);
	}

}

await using AsyncServiceScope asyncScope = scope.ServiceProvider.CreateAsyncScope();
IAnodeUOW anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();
string imagesPath = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
string extension = builder.Configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);
List<Dataset> dataset = await anodeUOW.Dataset.GetAll(
	[dataset => dataset.InstanceMatchID == int.Parse(instanceMatchID)]
	, withTracking: false);

foreach (var data in dataset)
{
	FileInfo sanFile = Shooting.GetImagePathFromRoot(
	data.CycleRID,
	data.StationID,
	imagesPath,
	data.AnodeType,
	data.CameraID,
	extension);

	int loadResponse = DLLVisionImport.fcx_load_anode(
		data.CameraID,
		sanFile.DirectoryName ?? string.Empty,
		Path.GetFileNameWithoutExtension(sanFile.Name));

	if (loadResponse != 0)
	{
		logger.LogError(
			"Failed to unload anode with response code {responseCode}.",
			loadResponse);
	}
}

host.Run();