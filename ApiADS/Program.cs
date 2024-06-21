using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Alarms.AlarmsRT.Services;
using Core.Entities.IOT.IOTDevices.Services;
using Core.Entities.IOT.IOTTags.Services;
using Core.Entities.Packets.Services;
using Core.Shared.Data;
using Core.Shared.Services.Background;
using Core.Shared.SignalR;
using Core.Shared.SignalR.IOTHub;
using Core.Shared.SignalR.StationCycleHub;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddRequiredBuilders();

builder.Services.AddSignalR();
builder.Services.AddScoped<ISignalRService, SignalRService>();

builder.Services.AddScoped<IAlarmLogService, AlarmLogService>();
builder.Services.AddScoped<IAlarmRTService, AlarmRTService>();
builder.Services.AddScoped<IPacketService, PacketService>();

builder.Services.AddScoped<IIOTDeviceService, IOTDeviceService>();
builder.Services.AddScoped<IIOTTagService, IOTTagService>();

builder.Services.AddSingleton<ADSService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<ADSService>());

builder.Services.AddSingleton<IOTService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<IOTService>());

WebApplication app = builder.Build();

app.AddRequiredApps();

app.MapHub<StationCycleHub>("/stationCycleHub");
app.MapHub<IOTHub>("/iotHub");

app.Run();