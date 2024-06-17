using Carter;
using Core.Configuration.Serilog;
using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.IOT.IOTDevices.Services;
using Core.Entities.IOT.IOTTags.Services;
using Core.Entities.Packets.Services;
using Core.Entities.Vision.ToDos.Services.ToMatchs;
using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background;
using Core.Shared.Services.SystemApp.Logs;
using Core.Shared.SignalR;
using Core.Shared.SignalR.IOTHub;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Configuration;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Use Serilog as logger
builder.Logging.ClearProviders();
builder.Host.UseSerilog(
	(ctx, serviceProvider, loggerConfig) => {
		loggerConfig
			.ReadFrom
			.Configuration(ctx.Configuration)
			.ReadFrom
			.Services(serviceProvider)
			.Enrich
			.WithCustomEnrichers(ctx.Configuration);
	});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.LoadBaseConfiguration();

builder.Services.AddDbContext<AnodeCTX>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionStringWithThrow("DefaultConnection")));

builder.Services.AddAuthentication(
	options => {
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	// Adding Jwt Bearer
	.AddJwtBearer(options => {
		options.SaveToken = true;
		options.RequireHttpsMetadata = false;
		string jwtSecret = builder.Configuration["JWT:Secret"]
			?? throw new ConfigurationErrorsException("Missing JWT Secret");
		options.TokenValidationParameters = new() {
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidAudience = builder.Configuration.GetValueWithThrow<string>("JWT:ValidAudience"),
			ValidIssuer = builder.Configuration.GetValueWithThrow<string>("JWT:ValidIssuer"),
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
		};
		options.Events = new() {
			OnMessageReceived = context => {
				if (context.Request.Query.TryGetValue("access_token", out StringValues token))
					context.Token = token;

				return Task.CompletedTask;
			},
			OnAuthenticationFailed = _ => Task.CompletedTask,
		};
	});

// To fix: Unable to resolve service for type 'Microsoft.AspNetCore.Http.IHttpContextAccessor'
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddSignalR();
builder.Services.AddScoped<ISignalRService, SignalRService>();

builder.Services.AddScoped<IIOTDeviceService, IOTDeviceService>();
builder.Services.AddScoped<IIOTTagService, IOTTagService>();

builder.Services.AddScoped<IAlarmLogService, AlarmLogService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IPacketService, PacketService>();
builder.Services.AddScoped<IAlarmCService, AlarmCService>();
builder.Services.AddScoped<IToMatchService, ToMatchService>();

builder.Services.AddSingleton<IOTService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<IOTService>());

//builder.Services.AddSingleton<DiskCheckService>();
//builder.Services.AddHostedService(provider => provider.GetRequiredService<DiskCheckService>());

builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();

builder.Services.AddCarter();

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

	//builder.Services.AddSingleton<PurgeService>();
	//builder.Services.AddHostedService(provider => provider.GetRequiredService<PurgeService>());
}

WebApplication app = builder.Build();

// Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

string[] clientHost = builder.Configuration.GetSectionWithThrow<string[]>(ConfigDictionary.ClientHost);
app.UseCors(corsPolicyBuilder => corsPolicyBuilder.WithOrigins(clientHost)
	.WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
	.AllowAnyHeader()
	.AllowCredentials());

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

app.MapHub<IOTHub>("/iotHub");

app.Run();