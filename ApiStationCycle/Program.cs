using System.Configuration;
using System.Text;
using Carter;
using Core.Entities.BenchmarkTests.Services;
using Core.Entities.Packets.Services;
using Core.Entities.StationCycles.Services;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background;
using Core.Shared.Services.System.Logs;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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

string? stationName = builder.Configuration.GetValue<string>("StationConfig:StationName");
if (stationName == null)
	throw new ConfigurationErrorsException("Missing StationConfig:StationName");
Station.Name = stationName;

string? address = builder.Configuration.GetValue<string>("ServerConfig:Address");
if (address == null)
	throw new ConfigurationErrorsException("Missing StationConfig:Address");
Station.ServerAddress = address;

builder.Services.AddAuthentication(options =>
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
		if (jwtSecret == null)
			throw new ConfigurationErrorsException("Missing JWT Secret");
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidAudience = builder.Configuration["JWT:ValidAudience"],
			ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
		};
		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				if (context.Request.Query.TryGetValue("access_token", out StringValues token)
				   )
					context.Token = token;

				return Task.CompletedTask;
			},
			OnAuthenticationFailed = _ => Task.CompletedTask
		};
	});

builder.Services.AddDbContext<AnodeCTX>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
	.AddEntityFrameworkStores<AnodeCTX>()
	.AddDefaultTokenProviders();

// To fix: Unable to resolve service for type 'Microsoft.AspNetCore.Http.IHttpContextAccessor'
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IBenchmarkTestService, BenchmarkTestService>();

builder.Services.AddScoped<IPacketService, PacketService>();
builder.Services.AddScoped<IStationCycleService, StationCycleService>();

builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();

builder.Services.AddCarter();

if (!Station.IsServer)
{
	builder.Services.AddSingleton<SendService>();
	builder.Services.AddHostedService(provider => provider.GetRequiredService<SendService>());

	builder.Services.AddSingleton<SendLogService>();
	builder.Services.AddHostedService(provider => provider.GetRequiredService<SendLogService>());
}

WebApplication app = builder.Build();

// Initialize
string? dbInitialize = builder.Configuration["DbInitialize"];
if (dbInitialize == null)
	throw new ConfigurationErrorsException("Missing DbInitialize");
if (!Station.IsServer && bool.Parse(dbInitialize))
{
	using IServiceScope scope = app.Services.CreateScope();
	IServiceProvider services = scope.ServiceProvider;
	AnodeCTX context = services.GetRequiredService<AnodeCTX>();
	UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
	await DBInitializer.InitializeStation(context, userManager);
}

app.UseSwagger();
app.UseSwaggerUI();

string? clientHost = builder.Configuration["ClientHost"];
if (clientHost == null)
	throw new ConfigurationErrorsException("Missing ClientHost");

app.UseCors(corsPolicyBuilder => corsPolicyBuilder.WithOrigins(clientHost)
	.WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
	.AllowAnyHeader()
	.AllowCredentials());

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapCarter();

app.Run();