using App.WindowsService;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Configuration;
using System.Text;
using Carter;
using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.IOT.IOTDevices.Services;
using Core.Entities.IOT.IOTTags.Services;
using Core.Entities.Packets.Services;
using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Background;
using Core.Shared.Services.System.Logs;
using Core.Shared.SignalR;
using Core.Shared.SignalR.IOTHub;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = ".NET Joke Service";
});

/*
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

        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        };
        options.Events = new()
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Query.TryGetValue("access_token", out StringValues token))
                    context.Token = token;

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = _ => Task.CompletedTask,
        };
    });


builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();
builder.Services.AddSignalR();


builder.Services.AddScoped<IAlarmLogService, AlarmLogService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IPacketService, PacketService>();
builder.Services.AddScoped<IAlarmCService, AlarmCService>();

builder.Services.AddSingleton<PurgeService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<PurgeService>());
*/


builder.Services.AddSingleton<JokeService>();
builder.Services.AddHostedService<WindowsBackgroundService>();

var host = builder.Build();
host.Run();
