using Core.Entities.Anodes.Services;
using Core.Entities.KPI.KPICs.Services;
using Core.Entities.KPI.KPIEntries.Services.KPILogs;
using Core.Entities.KPI.KPIEntries.Services.KPIRTs;
using Core.Entities.Packets.Services;
using Core.Entities.StationCycles.Services;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background.KPI.KPILogs;
using Core.Shared.Services.Background.KPI.KPIRTs;
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

string stationName = builder.Configuration.GetValue<string>("StationConfig:StationName");
Station.Name = stationName;

if (!Station.IsServer)
	throw new Exception("¨This API can only run on the server. Please update appsettings.json");

builder.Services.AddDbContext<AnodeCTX>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// To fix: Unable to resolve service for type 'Microsoft.AspNetCore.Http.IHttpContextAccessor'
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IKPICService, KPICService>();
builder.Services.AddScoped<IKPIRTService, KPIRTService>();
builder.Services.AddScoped<IKPILogService, KPILogService>();

builder.Services.AddScoped<IPacketService, PacketService>();
builder.Services.AddScoped<IStationCycleService, StationCycleService>();
//builder.Services.AddSingleton<HourlyStationCycleService>();
//builder.Services.AddHostedService(provider => provider.GetRequiredService<HourlyStationCycleService>());

builder.Services.AddScoped<IAnodeService, AnodeService>();
builder.Services.AddSingleton<HourlyAnodeService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<HourlyAnodeService>());

builder.Services.AddSingleton<DailyKPILogService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DailyKPILogService>());

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

if (bool.Parse(builder.Configuration["DbInitialize"]))
{
	using IServiceScope scope = app.Services.CreateScope();
	IServiceProvider services = scope.ServiceProvider;
	AnodeCTX context = services.GetRequiredService<AnodeCTX>();
	await DBInitializer.InitializeServer(context);
}

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