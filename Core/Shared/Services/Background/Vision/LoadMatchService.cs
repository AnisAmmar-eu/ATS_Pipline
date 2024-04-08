using Core.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using System.IO;
using Core.Shared.Dictionaries;
using System.Configuration;
using DLLVision;
using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Services.ToSigns;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Entities.Vision.ToDos.Services.ToMatchs;

namespace Core.Shared.Services.Background.Vision;

public class LoadService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<MatchService> _logger;
	private readonly IConfiguration _configuration;

	public LoadService(
		ILogger<LoadService> logger,
		IServiceScopeFactory factory,
		IConfiguration configuration)
	{
		_logger = logger;
		_factory = factory;
		_configuration = configuration;
	}

	const int dataset = 0;

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		IAnodeUOW _anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();

		string _imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string _extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);
		List<InstanceMatchID> UnloadDestinations = _configuration.GetSectionWithThrow<List<InstanceMatchID>>(
			ConfigDictionary.UnloadDestinations);

		int signMatchTimer = _configuration.GetValueWithThrow<int>(ConfigDictionary.SignMatchTimer);
		using PeriodicTimer timer = new(TimeSpan.FromSeconds(signMatchTimer));

		IToMatchService toMatchService
	   = asyncScope.ServiceProvider.GetRequiredService<IToMatchService>();

		while (await timer.WaitForNextTickAsync(stoppingToken)
			   && !stoppingToken.IsCancellationRequested)
		{
			await _anodeUOW.StartTransaction();

			try
			{
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute MatchService with exception message {message}.",
					ex.Message);
			}

			await _anodeUOW.CommitTransaction();
		}
	}
}