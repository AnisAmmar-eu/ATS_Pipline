using System.Text;
using Carter;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Shared.Configuration;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Core.Shared.Data;

public static class DependencyInjection
{
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

		// To fix: Unable to resolve service for type 'Microsoft.AspNetCore.Http.IHttpContextAccessor'
		services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		services.AddIdentity<ApplicationUser, ApplicationRole>()
			.AddEntityFrameworkStores<AnodeCTX>()
			.AddDefaultTokenProviders();

		services.AddScoped<IAnodeUOW, AnodeUOW>();
		services.AddCarter();

		return services;
	}
}