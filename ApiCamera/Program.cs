using Core.Entities.BI.BITemperatures.Services;
using Core.Entities.IOT.IOTDevices.Services;
using Core.Entities.IOT.IOTTags.Services;
using Core.Entities.Packets.Services;
using Core.Shared.Data;
using Core.Shared.Services.Background;
using Core.Shared.Services.Background.BI.BITemperature;
using Core.Shared.Services.SystemApp.Logs;
using Core.Shared.SignalR;
using Core.Shared.SignalR.CameraHub;
using Core.Shared.SignalR.IOTHub;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddRequiredBuilders();

builder.Services.AddSignalR();
builder.Services.AddScoped<ISignalRService, SignalRService>();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IIOTDeviceService, IOTDeviceService>();
builder.Services.AddScoped<IIOTTagService, IOTTagService>();
builder.Services.AddScoped<IBITemperatureService, BITemperatureService>();
builder.Services.AddScoped<IPacketService, PacketService>();

builder.Services.AddSingleton<TemperatureService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<TemperatureService>());

builder.Services.AddSingleton<IOTService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<IOTService>());

builder.Services.AddSingleton<CameraService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<CameraService>());

WebApplication app = builder.Build();

app.AddRequiredApps();

app.MapHub<IOTHub>("/iotHub");
app.MapHub<CameraHub>("/cameraHub");

app.Run();