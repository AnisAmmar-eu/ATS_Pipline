using Core.Entities.Alarms.AlarmsPLC.Services;
using Core.Entities.IOT.IOTDevices.Services;
using Core.Entities.IOT.IOTTags.Services;
using Core.Entities.Packets.Services;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background;
using Core.Shared.SignalR;
using Core.Shared.SignalR.StationCycleHub;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string stationName = builder.Configuration.GetValue<string>("StationConfig:StationName");
Station.Name = stationName;

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AnodeCTX>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddScoped<IAlarmPLCService, AlarmPLCService>();
builder.Services.AddScoped<IPacketService, PacketService>();

builder.Services.AddScoped<IIOTDeviceService, IOTDeviceService>();
builder.Services.AddScoped<IIOTTagService, IOTTagService>();

builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();

builder.Services.AddSignalR();
builder.Services.AddScoped<ISignalRService, SignalRService>();

builder.Services.AddSingleton<IServiceProvider>(sp => sp);

builder.Services.AddSingleton<ADSService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<ADSService>());

builder.Services.AddSingleton<IOTService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<IOTService>());

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapHub<StationCycleHub>("/stationCycleHub");

app.Run();