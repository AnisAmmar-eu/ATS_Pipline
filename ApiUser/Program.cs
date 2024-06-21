using System.Reflection;
using ApiUser.SwaggerConfig;
using Core.Entities.User.Dictionaries;
using Core.Entities.User.Services.Acts;
using Core.Entities.User.Services.Auth;
using Core.Entities.User.Services.Roles;
using Core.Entities.User.Services.Users;
using Core.Shared.Authorize;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Jwt;
using Core.Shared.Services.SystemApp.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddRequiredBuilders();

if (!Station.IsServer)
	throw new("¨This API can only run on the server. Please update appsettings.json");

// System services
builder.Services.AddScoped<ILogService, LogService>();

// User Services
builder.Services.AddScoped<IActService, ActService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAuthorizationBuilder()
	.AddPolicy(
		ActionRID.AdminGeneralRights,
		policy => policy.AddRequirements(new ActAuthorize(ActionRID.AdminGeneralRights)));

builder.Services.AddScoped<IAuthorizationHandler, ActAuthorizeHandler>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
	options => {
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

app.AddRequiredApps();

app.Run();