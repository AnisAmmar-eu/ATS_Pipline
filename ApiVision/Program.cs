using Carter;
using Core.Entities.Vision.FileSettings.Services;
using Core.Entities.Vision.SignedCycles.Services.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Services.MatchableStacks;
using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background.Vision;
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

builder.Configuration.LoadBaseConfiguration();

if (!Station.IsServer)
	throw new("¨This API can only run on the server. Please update appsettings.json");

builder.Services.AddDbContext<AnodeCTX>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionStringWithThrow("DefaultConnection")));

// To fix: Unable to resolve service for type 'Microsoft.AspNetCore.Http.IHttpContextAccessor'
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IFileSettingService, FileSettingService>();
builder.Services.AddScoped<ILoadableQueueService, LoadableQueueService>();
builder.Services.AddScoped<IMatchableStackService, MatchableStackService>();

builder.Services.AddSingleton<LoadMatchService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<LoadMatchService>());

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