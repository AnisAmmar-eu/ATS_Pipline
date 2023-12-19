using System.Configuration;
using System.Text;
using Carter;
using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Alarms.AlarmsRT.Services;
using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Services.System.Logs;
using Core.Shared.SignalR;
using Core.Shared.SignalR.AlarmHub;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.LoadBaseConfiguration();

builder.Services.AddDbContext<AnodeCTX>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionStringWithThrow("DefaultConnection")));

builder.Services.AddAuthentication(
	options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	})
// Adding Jwt Bearer
	.AddJwtBearer(options =>
	{
		options.SaveToken = true;
		options.RequireHttpsMetadata = false;
		string? jwtSecret = builder.Configuration["JWT:Secret"];
		if (jwtSecret is null)
			throw new ConfigurationErrorsException("Missing JWT Secret");

		options.TokenValidationParameters = new() {
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidAudience = builder.Configuration["JWT:ValidAudience"],
			ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
		};
		options.Events = new() {
			OnMessageReceived = context =>
			{
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

builder.Services.AddScoped<IAlarmCService, AlarmCService>();
builder.Services.AddScoped<IAlarmLogService, AlarmLogService>();
builder.Services.AddScoped<IAlarmRTService, AlarmRTService>();

builder.Services.AddSignalR();
builder.Services.AddScoped<ISignalRService, SignalRService>();

builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

app.MapHub<AlarmHub>("/alarmsHub");

app.Run();