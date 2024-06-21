using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.IOT.IOTDevices.Services;
using Core.Entities.IOT.IOTTags.Services;
using Core.Entities.Packets.Services;
using Core.Entities.Vision.ToDos.Services.ToMatchs;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background;
using Core.Shared.Services.SystemApp.Logs;
using Core.Shared.SignalR;
using Core.Shared.SignalR.IOTHub;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddRequiredBuilders();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddSignalR();
builder.Services.AddScoped<ISignalRService, SignalRService>();

builder.Services.AddScoped<IIOTDeviceService, IOTDeviceService>();
builder.Services.AddScoped<IIOTTagService, IOTTagService>();

builder.Services.AddScoped<IAlarmLogService, AlarmLogService>();
builder.Services.AddScoped<IPacketService, PacketService>();
builder.Services.AddScoped<IAlarmCService, AlarmCService>();
builder.Services.AddScoped<IToMatchService, ToMatchService>();

builder.Services.AddSingleton<IOTService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<IOTService>());

if (Station.IsServer)
{
	builder.Services.AddSingleton<PurgeServer>();
	builder.Services.AddHostedService(provider => provider.GetRequiredService<PurgeServer>());

	builder.Services.AddSingleton<NotifyService>();
	builder.Services.AddHostedService(provider => provider.GetRequiredService<NotifyService>());
}
else
{
	builder.Services.AddSingleton<CheckSyncTimeService>();
	builder.Services.AddHostedService(provider => provider.GetRequiredService<CheckSyncTimeService>());

	builder.Services.AddSingleton<DiskCheckService>();
	builder.Services.AddHostedService(provider => provider.GetRequiredService<DiskCheckService>());

	builder.Services.AddSingleton<PurgeService>();
	builder.Services.AddHostedService(provider => provider.GetRequiredService<PurgeService>());
}

WebApplication app = builder.Build();

app.AddRequiredApps();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHub<IOTHub>("/iotHub");

app.Run();