using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Alarms.AlarmsRT.Services;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background;
using Core.Shared.Services.SystemApp.Logs;
using Core.Shared.SignalR;
using Core.Shared.SignalR.AlarmHub;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddRequiredBuilders();

builder.Services.AddSignalR();
builder.Services.AddScoped<ISignalRService, SignalRService>();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IAlarmCService, AlarmCService>();
builder.Services.AddScoped<IAlarmLogService, AlarmLogService>();
builder.Services.AddScoped<IAlarmRTService, AlarmRTService>();

if (!Station.IsServer)
{
	builder.Services.AddSingleton<SendAlarmLogService>();
	builder.Services.AddHostedService(provider => provider.GetRequiredService<SendAlarmLogService>());
}

WebApplication app = builder.Build();

app.AddRequiredApps();

app.MapHub<AlarmHub>("/alarmsHub");

app.Run();