using System.Text;
using Carter;
using Core.Configuration.Serilog;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Core.Shared.Data;

public static class DependencyInjection
{
	/// <summary>
	/// Custom method for IServiceCollection to add our required services
	/// </summary>
	/// <param name="services"></param>
	/// <param name="configuration"></param>
	/// <returns></returns>
	public static IServiceCollection AddRequiredServices(this IServiceCollection services, IConfiguration configuration)
	{
		// Add services to the container.
		services.AddControllers();

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		configuration.LoadBaseConfiguration();

		services.AddAuthentication(
			options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			// Adding Jwt Bearer
			.AddJwtBearer(options => {
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				string jwtSecret = configuration.GetValueWithThrow<string>("JWT:Secret");
				options.TokenValidationParameters = new() {
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidAudience = configuration.GetValueWithThrow<string>("JWT:ValidAudience"),
					ValidIssuer = configuration.GetValueWithThrow<string>("JWT:ValidIssuer"),
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

		services.AddDbContext<AnodeCTX>(
			options => options.UseSqlServer(configuration.GetConnectionStringWithThrow("DefaultConnection")));

		services.AddIdentity<ApplicationUser, ApplicationRole>()
			.AddEntityFrameworkStores<AnodeCTX>()
			.AddDefaultTokenProviders();

		// To fix: Unable to resolve service for type 'Microsoft.AspNetCore.Http.IHttpContextAccessor'
		services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

		services.AddScoped<IAnodeUOW, AnodeUOW>();

		services.AddCarter();

		services.AddOutputCache();

		string[] clientHost = configuration.GetSectionWithThrow<string[]>(ConfigDictionary.ClientHost);
		services.AddCors(options => {
			options.AddDefaultPolicy(corsPolicyBuilder => corsPolicyBuilder.WithOrigins(clientHost)
				.WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
				.AllowAnyHeader()
				.AllowCredentials());
		});

		return services;
	}

	/// <summary>
	/// Custom method for WebApplicationBuilder to add our required builders
	/// </summary>
	/// <param name="builder"></param>
	/// <returns></returns>
	public static WebApplicationBuilder AddRequiredBuilders(this WebApplicationBuilder builder)
	{
		// Set appsettings configuration
		if (builder.Environment.IsDevelopment())
		{
			string sharedFolderPath = Path.Combine(builder.Environment.ContentRootPath, "..", "_Shared");
			builder.Configuration
				.AddJsonFile(
					Path.Combine(sharedFolderPath, $"appsettings.Shared.{builder.Environment.EnvironmentName}.json"),
					optional: true);
		}
		else
		{
			builder.Configuration
				.AddJsonFile($"appsettings.Shared.{builder.Environment.EnvironmentName}.json", optional: true);
		}

		builder.Configuration
			.AddJsonFile("appsettings.json", optional: true)
			.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

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

		builder.Services.AddRequiredServices(builder.Configuration);

		return builder;
	}

	/// <summary>
	/// Custom method for WebApplication to add our required apps
	/// </summary>
	/// <param name="app"></param>
	/// <returns></returns>
	public static WebApplication AddRequiredApps(this WebApplication app)
	{
		// Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseCors();

		app.UseHttpsRedirection();

		app.UseRouting();

		app.UseAuthentication();
		app.UseAuthorization();

		app.MapCarter();

		app.UseOutputCache();

		return app;
	}
}