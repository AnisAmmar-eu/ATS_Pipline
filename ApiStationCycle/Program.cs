using System.Configuration;
using Core.Entities.Anodes.Services;
using Core.Entities.BenchmarkTests.Services;
using Core.Entities.DebugsModes.Services;
using Core.Entities.Packets.Services;
using Core.Entities.StationCycles.Services;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.Vision.ToDos.Services.ToMatchs;
using Core.Entities.Vision.ToDos.Services.ToSigns;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background;
using Core.Shared.Services.SystemApp.Logs;
using Microsoft.AspNetCore.Identity;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddRequiredBuilders();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IBenchmarkTestService, BenchmarkTestService>();
builder.Services.AddScoped<IPacketService, PacketService>();
builder.Services.AddScoped<IStationCycleService, StationCycleService>();
builder.Services.AddScoped<IAnodeService, AnodeService>();
builder.Services.AddScoped<IDebugModeService, DebugModeService>();
builder.Services.AddScoped<IToMatchService, ToMatchService>();
builder.Services.AddScoped<IToSignService, ToSignService>();

if (!Station.IsServer)
{
	builder.Services.AddSingleton<SendPacketService>();
	builder.Services.AddHostedService(provider => provider.GetRequiredService<SendPacketService>());

	builder.Services.AddSingleton<SendLogService>();
	builder.Services.AddHostedService(provider => provider.GetRequiredService<SendLogService>());
}

WebApplication app = builder.Build();

// Initialize
string? dbInitialize = builder.Configuration["DbInitialize"]
	?? throw new ConfigurationErrorsException("Missing DbInitialize");

if (!Station.IsServer && bool.Parse(dbInitialize))
{
	using IServiceScope scope = app.Services.CreateScope();
	IServiceProvider services = scope.ServiceProvider;
	AnodeCTX context = services.GetRequiredService<AnodeCTX>();
	UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
	await DBInitializer.InitializeStation(context, userManager);
}

app.AddRequiredApps();

app.Run();