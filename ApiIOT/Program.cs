using Core.Entities.IOT.IOTDevices.Services;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Services;
using Core.Shared.Data;
using Core.Shared.Services.System.Logs;
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


builder.Services.AddDbContext<AnodeCTX>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// To fix: Unable to resolve service for type 'Microsoft.AspNetCore.Http.IHttpContextAccessor'
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<ILogsService, LogsService>();
builder.Services.AddScoped<IIOTDeviceService, IOTDeviceService>();
builder.Services.AddScoped<IIOTTagService, IOTTagService>();

builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

string? clientHost = builder.Configuration["ClientHost"];

app.UseCors(corsPolicyBuilder => corsPolicyBuilder.WithOrigins(clientHost)
	.WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
	.AllowAnyHeader()
	.AllowCredentials());

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();