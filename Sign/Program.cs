using Core.Shared.Configuration;
using Core.Shared.Services.Background.Vision;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
	options.ServiceName = "Sign service";
});


builder.Configuration.LoadBaseConfiguration();

//builder.Services.AddDbContext<AnodeCTX>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionStringWithThrow("DefaultConnection")));

builder.Services.AddSingleton<SignService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<SignService>());

var host = builder.Build();
host.Run();
