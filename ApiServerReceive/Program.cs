using System.Configuration;
using Carter;
using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Alarms.AlarmsRT.Services;
using Core.Entities.Packets.Services;
using Core.Entities.StationCycles.Services;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.SystemApp.Logs;
using Core.Shared.SignalR;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.LoadBaseConfiguration();

if (!Station.IsServer)
	throw new("¨This API can only run on the server. Please update appsettings.json");

builder.Services.AddDbContext<AnodeCTX>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionStringWithThrow("DefaultConnection")));

// To fix: Unable to resolve service for type 'Microsoft.AspNetCore.Http.IHttpContextAccessor'
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
	.AddEntityFrameworkStores<AnodeCTX>()
	.AddDefaultTokenProviders();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IAlarmCService, AlarmCService>();
builder.Services.AddScoped<IAlarmLogService, AlarmLogService>();
builder.Services.AddScoped<IAlarmRTService, AlarmRTService>();
builder.Services.AddScoped<IPacketService, PacketService>();
builder.Services.AddScoped<IStationCycleService, StationCycleService>();

builder.Services.AddSignalR();
builder.Services.AddScoped<ISignalRService, SignalRService>();

builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();

builder.Services.AddCarter();

builder.Services.AddCors(
	options =>
	{
		options.AddPolicy(
			"AllowOrigin",
			policyBuilder =>
			{
				policyBuilder.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader();
			});
	});

WebApplication app = builder.Build();

string? dbInitialize = builder.Configuration["DbInitialize"];
if (dbInitialize is null)
	throw new ConfigurationErrorsException("Missing DbInitialize");
if (bool.Parse(dbInitialize))
{
	using IServiceScope scope = app.Services.CreateScope();
	IServiceProvider services = scope.ServiceProvider;
	AnodeCTX context = services.GetRequiredService<AnodeCTX>();
	UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
	await DBInitializer.InitializeServer(context, userManager);
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

app.MapCarter();

app.Run();