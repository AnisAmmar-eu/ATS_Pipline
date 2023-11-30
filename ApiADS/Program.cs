using System.Configuration;
using Carter;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.IOT.IOTDevices.Services;
using Core.Entities.IOT.IOTTags.Services;
using Core.Entities.Packets.Services;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background;
using Core.Shared.SignalR;
using Core.Shared.SignalR.IOTHub;
using Core.Shared.SignalR.StationCycleHub;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? stationName = builder.Configuration.GetValue<string>("StationConfig:StationName");
if (stationName is null)
	throw new ConfigurationErrorsException("Missing StationConfig:StationName");
Station.Name = stationName;

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString is null)
	throw new ConfigurationErrorsException("Missing DefaultConnection");

builder.Services.AddDbContext<AnodeCTX>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddScoped<IAlarmLogService, AlarmLogService>();
builder.Services.AddScoped<IPacketService, PacketService>();

builder.Services.AddScoped<IIOTDeviceService, IOTDeviceService>();
builder.Services.AddScoped<IIOTTagService, IOTTagService>();

builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();

builder.Services.AddSignalR();
builder.Services.AddScoped<ISignalRService, SignalRService>();

builder.Services.AddSingleton<IServiceProvider>(sp => sp);

builder.Services.AddSingleton<ADSService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<ADSService>());

builder.Services.AddSingleton<IOTService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<IOTService>());

builder.Services.AddCarter();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapCarter();

app.MapHub<StationCycleHub>("/stationCycleHub");
app.MapHub<IOTHub>("/iotHub");

app.Run();