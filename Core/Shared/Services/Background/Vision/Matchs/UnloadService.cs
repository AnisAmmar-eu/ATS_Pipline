using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.DLLVision;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background.Vision.Matchs;

public class UnloadService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<UnloadService> _logger;
	private readonly IConfiguration _configuration;

	public UnloadService(
		ILogger<UnloadService> logger,
		IServiceScopeFactory factory,
		IConfiguration configuration)
	{
		_logger = logger;
		_factory = factory;
		_configuration = configuration;
	}

	/// <summary>Unload in dataset</summary>
	/// <summary>Need to get all ToUnload with InstanceMatchID</summary>
	/// <summary>Then use DLLVision to unload</summary>
	/// <summary>Update dataset having deleted unload line</summary>
	/// <summary>Then delete unload line from Unload table</summary>
	/// <param name="stoppingToken">Cancellation token</param>
	/// <returns>Task</returns>
	/// <exception cref="Exception">Failed to execute UnloadService with exception message {message}.</exception>
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		string imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);
		int instanceMatchID = _configuration.GetValueWithThrow<int>(ConfigDictionary.InstanceMatchID);

		int signMatchTimer = _configuration.GetValueWithThrow<int>(ConfigDictionary.SignMatchTimer);

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromSeconds(signMatchTimer), stoppingToken);
			await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
			IAnodeUOW anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();

			try
			{
				List<ToUnload> toUnloads = await anodeUOW.ToUnload.GetAll(
					[unload => unload.InstanceMatchID == instanceMatchID],
					withTracking: false);

				foreach (ToUnload toUnload in toUnloads)
				{
					foreach (int cameraID in new int[] { 1, 2 })
					{
						string sANFile = Shooting.GetImagePathFromRoot(
							toUnload.CycleRID,
							toUnload.StationID,
							imagesPath,
							toUnload.AnodeType,
							cameraID,
							extension).FullName;

						int dropResponse = DLLVisionImport.fcx_drop_anode(
							cameraID,
							sANFile);

						if (dropResponse != 0)
						{
							_logger.LogError(
								"Failed to unload anode with response code {responseCode}.",
								dropResponse);
						}
					}

					await anodeUOW.Dataset.ExecuteDeleteAsync(
						dataset => dataset.CycleRID == toUnload.CycleRID
							&& toUnload.InstanceMatchID == dataset.InstanceMatchID);

					await anodeUOW.ToUnload.ExecuteDeleteAsync(
						unload => unload.ID == toUnload.ID);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute UnloadService with exception message {message}.",
					ex.Message);
			}
		}
	}
}