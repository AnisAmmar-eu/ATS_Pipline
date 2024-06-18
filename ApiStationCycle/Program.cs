using System.Configuration;
using System.Text;
using Carter;
using Core.Configuration.Serilog;
using Core.Entities.Anodes.Services;
using Core.Entities.BenchmarkTests.Services;
using Core.Entities.DebugsModes.Services;
using Core.Entities.Packets.Services;
using Core.Entities.StationCycles.Services;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.Vision.ToDos.Services.ToMatchs;
using Core.Entities.Vision.ToDos.Services.ToSigns;
using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background;
using Core.Shared.Services.SystemApp.Logs;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Serilog;

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
		string jwtSecret = builder.Configuration.GetValueWithThrow<string>("JWT:Secret");
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

builder.Services.AddDbContext<AnodeCTX>(
	options => options.UseSqlServer(builder.Configuration.GetConnectionStringWithThrow("DefaultConnection")));

// To fix: Unable to resolve service for type 'Microsoft.AspNetCore.Http.IHttpContextAccessor'
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
	.AddEntityFrameworkStores<AnodeCTX>()
	.AddDefaultTokenProviders();

builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IBenchmarkTestService, BenchmarkTestService>();
builder.Services.AddScoped<IPacketService, PacketService>();
builder.Services.AddScoped<IStationCycleService, StationCycleService>();
builder.Services.AddScoped<IAnodeService, AnodeService>();
builder.Services.AddScoped<IDebugModeService, DebugModeService>();
builder.Services.AddScoped<IToMatchService, ToMatchService>();
builder.Services.AddScoped<IToSignService, ToSignService>();
builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();

builder.Services.AddCarter();

builder.Services.AddOutputCache();

if (!Station.IsServer)
{
	builder.Services.AddSingleton<SendPacketService>();
	builder.Services.AddHostedService(provider => provider.GetRequiredService<SendPacketService>());

	builder.Services.AddSingleton<SendLogService>();
	builder.Services.AddHostedService(provider => provider.GetRequiredService<SendLogService>());
}

WebApplication app = builder.Build();

// Initialize
string? dbInitialize = builder.Configuration["DbInitialize"]
	?? throw new ConfigurationErrorsException("Missing DbInitialize");

if (!Station.IsServer && bool.Parse(dbInitialize))
{
	using IServiceScope scope = app.Services.CreateScope();
	IServiceProvider services = scope.ServiceProvider;
	AnodeCTX context = services.GetRequiredService<AnodeCTX>();
	UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
	await DBInitializer.InitializeStation(context, userManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

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

app.UseOutputCache();

app.Run();