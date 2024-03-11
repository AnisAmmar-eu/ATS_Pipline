using Core.Entities.Anodes.Services;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Shared.Configuration;
using Core.Shared.Data;
using Core.Shared.Dictionaries;
using Core.Shared.Services.System.Logs;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sign;
using System.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.LoadBaseConfiguration();


builder.Services.AddDbContext<AnodeCTX>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionStringWithThrow("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AnodeCTX>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IAnodeUOW, AnodeUOW>();
builder.Services.AddScoped<IAnodeService, AnodeService>();

var host = builder.Build();


string? dbInitialize = builder.Configuration["DbInitialize"];
if (dbInitialize is null)
    throw new ConfigurationErrorsException("Missing DbInitialize");
if (bool.Parse(dbInitialize))
{
    using IServiceScope scope = host.Services.CreateScope();
    IServiceProvider services = scope.ServiceProvider;
    AnodeCTX context = services.GetRequiredService<AnodeCTX>();
    UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    await DBInitializer.InitializeServer(context, userManager);
}


builder.Services.AddHostedService<Worker>();


host.Run();
