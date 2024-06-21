using System.Configuration;
using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Alarms.AlarmsRT.Services;
using Core.Entities.Packets.Services;
using Core.Entities.StationCycles.Services;
using Core.Entities.User.Models.DB.Users;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.SystemApp.Logs;
using Core.Shared.SignalR;
using Microsoft.AspNetCore.Identity;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddRequiredBuilders();

if (!Station.IsServer)
	throw new("¨This API can only run on the server. Please update appsettings.json");

builder.Services.AddSignalR();
builder.Services.AddScoped<ISignalRService, SignalRService>();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IAlarmCService, AlarmCService>();
builder.Services.AddScoped<IAlarmLogService, AlarmLogService>();
builder.Services.AddScoped<IAlarmRTService, AlarmRTService>();
builder.Services.AddScoped<IPacketService, PacketService>();
builder.Services.AddScoped<IStationCycleService, StationCycleService>();

WebApplication app = builder.Build();

string? dbInitialize = builder.Configuration["DbInitialize"]
	?? throw new ConfigurationErrorsException("Missing DbInitialize");

if (bool.Parse(dbInitialize))
{
	using IServiceScope scope = app.Services.CreateScope();
	IServiceProvider services = scope.ServiceProvider;
	AnodeCTX context = services.GetRequiredService<AnodeCTX>();
	UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
	await DBInitializer.InitializeServer(context, userManager);
}

app.AddRequiredApps();

app.Run();