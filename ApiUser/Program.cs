using System.Reflection;
using System.Text;
using ApiUser.SwaggerConfig;
using Carter;
using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.User.Services.Acts;
using Core.Entities.User.Services.Auth;
using Core.Entities.User.Services.Roles;
using Core.Entities.User.Services.Users;
using Core.Shared.Authorize;
using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Jwt;
using Core.Shared.Services.SystemApp.Logs;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Configuration.LoadBaseConfiguration();

if (!Station.IsServer)
	throw new("¨This API can only run on the server. Please update appsettings.json");

builder.Services.AddDbContext<AnodeCTX>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionStringWithThrow("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
	options =>
	{
		options.Password.RequiredLength = 1;
		options.Password.RequireLowercase = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireDigit = false;
	})
	.AddEntityFrameworkStores<AnodeCTX>()
	.AddDefaultTokenProviders();

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
		string jwtSecret = builder.Configuration.GetValueWithThrow<string>("JWT:Secret");
		options.TokenValidationParameters = new() {
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidAudience = builder.Configuration.GetValueWithThrow<string>("JWT:ValidAudience"),
			ValidIssuer = builder.Configuration.GetValueWithThrow<string>("JWT:ValidIssuer"),
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
		};
		options.Events = new() {
			OnMessageReceived = context =>
			{
				if (context.Request.Query.TryGetValue("access_token", out StringValues token)
				   )
					context.Token = token;

				return Task.CompletedTask;
			},
			OnAuthenticationFailed = _ => Task.CompletedTask,
		};
	});

// To fix: Unable to resolve service for type 'Microsoft.AspNetCore.Http.IHttpContextAccessor'
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// User Services
builder.Services.AddScoped<IActService, ActService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();

// System services
builder.Services.AddScoped<ILogService, LogService>();

// UnitOfWork
builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();

builder.Services.AddCarter();

builder.Services.AddAuthorizationBuilder()
	.AddPolicy(
		ActionRID.AdminGeneralRights,
		policy => policy.AddRequirements(new ActAuthorize(ActionRID.AdminGeneralRights)));

builder.Services.AddScoped<IAuthorizationHandler, ActAuthorizeHandler>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
	options =>
	{
		options.SwaggerDoc(
			"v1",
			new OpenApiInfo {
				Version = "v1",
				Title = "ApiUser",
				Description = "An Api to manage user actions",
			});
		// using System.Reflection;
		string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
		options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

		options.AddSecurityDefinition(
			"Bearer",
			new OpenApiSecurityScheme {
				Name = "Authorization",
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer",
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
				Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your"
					+ " token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
			});
		options.AddSecurityRequirement(new OpenApiSecurityRequirement {
			{
				new OpenApiSecurityScheme {
					Reference = new() {
						Id = "Bearer",
						Type = ReferenceType.SecurityScheme,
					},
				},
				Array.Empty<string>()
			},
		});
		options.OperationFilter<SwaggerActionHeader>();
	});

WebApplication app = builder.Build();

string[] clientHost = builder.Configuration.GetSectionWithThrow<string[]>(ConfigDictionary.ClientHost);
app.UseCors(policyBuilder => policyBuilder.WithOrigins(clientHost)
	.WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
	.AllowAnyHeader()
	.AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

app.Run();