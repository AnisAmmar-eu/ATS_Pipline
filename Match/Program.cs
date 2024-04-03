using Core.Shared.Configuration;
using Core.Shared.Services.Background.Vision;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "Match service";
});


builder.Configuration.LoadBaseConfiguration();

//builder.Services.AddDbContext<AnodeCTX>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionStringWithThrow("DefaultConnection")));

builder.Services.AddSingleton<MatchService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<MatchService>());

builder.Services.AddSingleton<SignFileSettingService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<SignFileSettingService>());

var host = builder.Build();
host.Run();
