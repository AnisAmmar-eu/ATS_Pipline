using System.Configuration;
using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Alarms.AlarmsRT.Services;
using Core.Entities.Packets.Services;
using Core.Entities.StationCycles.Services;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.System.Logs;
using Core.Shared.SignalR;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? stationName = builder.Configuration.GetValue<string>("StationConfig:StationName");
if (stationName == null)
	throw new ConfigurationErrorsException("Missing StationConfig:StationName");
Station.Name = stationName;

if (!Station.IsServer)
	throw new Exception("¨This API can only run on the server. Please update appsettings.json");

builder.Services.AddDbContext<AnodeCTX>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// To fix: Unable to resolve service for type 'Microsoft.AspNetCore.Http.IHttpContextAccessor'
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IAlarmCService, AlarmCService>();
builder.Services.AddScoped<IAlarmLogService, AlarmLogService>();
builder.Services.AddScoped<IAlarmRTService, AlarmRTService>();
builder.Services.AddScoped<IPacketService, PacketService>();
builder.Services.AddScoped<IStationCycleService, StationCycleService>();

builder.Services.AddSignalR();
builder.Services.AddScoped<ISignalRService, SignalRService>();

builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();


builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowOrigin", policyBuilder =>
	{
		policyBuilder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});


WebApplication app = builder.Build();

app.UseCors("AllowOrigin");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();