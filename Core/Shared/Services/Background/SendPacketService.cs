using Core.Entities.Packets.Services;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

/// <summary>
/// Background service responsible for sending completed packets.
/// If the packet is a Shooting one, its images will be sent along.
/// </summary>
public class SendPacketService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<SendPacketService> _logger;
	private readonly IConfiguration _configuration;

	public SendPacketService(ILogger<SendPacketService> logger, IServiceScopeFactory factory, IConfiguration configuration)
	{
		_logger = logger;
		_factory = factory;
		_configuration = configuration;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		int delayMS = _configuration.GetValueWithThrow<int>("SendPacketMS");
		using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(delayMS));
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		IConfiguration configuration = asyncScope.ServiceProvider.GetRequiredService<IConfiguration>();
		string imagesPath = configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);

		IPacketService packetService
			= asyncScope.ServiceProvider.GetRequiredService<IPacketService>();

		while (await timer.WaitForNextTickAsync(stoppingToken)
			&& !stoppingToken.IsCancellationRequested)
        {
            try
			{
				await packetService.SendCompletedPackets(imagesPath);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute PeriodicSendService with exception message {message}. Good luck next round!",
					ex.Message);
			}
        }
    }
}