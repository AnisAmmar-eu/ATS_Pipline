using System.Configuration;
using Carter;
using Core.Entities.BI.BITemperatures.Services;
using Core.Entities.IOT.IOTDevices.Services;
using Core.Entities.IOT.IOTTags.Services;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background;
using Core.Shared.Services.Background.BI.BITemperature;
using Core.Shared.Services.System.Logs;
using Core.Shared.SignalR;
using Core.Shared.SignalR.IOTHub;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? stationName = builder.Configuration.GetValue<string>("StationConfig:StationName");
if (stationName is null)
	throw new ConfigurationErrorsException("Missing StationConfig:StationName");
Station.Name = stationName;

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AnodeCTX>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddSignalR();
builder.Services.AddScoped<ISignalRService, SignalRService>();

builder.Services.AddScoped<IIOTDeviceService, IOTDeviceService>();
builder.Services.AddScoped<IIOTTagService, IOTTagService>();
builder.Services.AddScoped<IBITemperatureService, BITemperatureService>();

builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();

builder.Services.AddSingleton<TemperatureService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<TemperatureService>());

builder.Services.AddSingleton<IOTService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<IOTService>());

builder.Services.AddSingleton<CameraService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<CameraService>());

builder.Services.AddCarter();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

string? clientHost = builder.Configuration["ClientHost"];
if (clientHost is null)
	throw new ConfigurationErrorsException("Missing ClientHost");

app.UseCors(corsPolicyBuilder => corsPolicyBuilder.WithOrigins(clientHost)
	.WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
	.AllowAnyHeader()
	.AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapCarter();

app.MapHub<IOTHub>("/iotHub");

app.Run();