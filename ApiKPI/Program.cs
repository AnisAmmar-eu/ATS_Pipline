using Core.Entities.Anodes.Services;
using Core.Entities.KPIData.KPIs.Services;
using Core.Entities.Packets.Services;
using Core.Entities.StationCycles.Services;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.SystemApp.Logs;
using Core.Shared.SignalR;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddRequiredBuilders();

if (!Station.IsServer)
	throw new("¨This API can only run on the server. Please update appsettings.json");

builder.Services.AddSignalR();
builder.Services.AddScoped<ISignalRService, SignalRService>();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IKPIService, KPIService>();

builder.Services.AddScoped<IAnodeService, AnodeService>();
builder.Services.AddScoped<IPacketService, PacketService>();
builder.Services.AddScoped<IStationCycleService, StationCycleService>();

WebApplication app = builder.Build();

app.AddRequiredApps();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();