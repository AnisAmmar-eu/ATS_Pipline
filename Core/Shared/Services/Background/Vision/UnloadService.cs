using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DLLVision;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;

namespace Core.Shared.Services.Background.Vision;

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
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		IAnodeUOW _anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();

		string _imagesPath = _configuration.GetValueWithThrow<string>(ConfigDictionary.ImagesPath);
		string _extension = _configuration.GetValueWithThrow<string>(ConfigDictionary.CameraExtension);
		List<InstanceMatchID> UnloadDestinations = _configuration.GetSectionWithThrow<List<InstanceMatchID>>(
			ConfigDictionary.UnloadDestinations);
		InstanceMatchID instanceMatchID = _configuration.GetValueWithThrow<InstanceMatchID>(ConfigDictionary.InstanceMatchID);
		string anodeType = _configuration.GetValueWithThrow<string>(ConfigDictionary.AnodeType);
		int cameraID = _configuration.GetValueWithThrow<int>(ConfigDictionary.CameraID);

		int signMatchTimer = _configuration.GetValueWithThrow<int>(ConfigDictionary.SignMatchTimer);
		using PeriodicTimer timer = new(TimeSpan.FromSeconds(signMatchTimer));

		while (await timer.WaitForNextTickAsync(stoppingToken)
			&& !stoppingToken.IsCancellationRequested)
		{
			try
			{
				List<ToUnload> toUnloads = await _anodeUOW.ToUnload.GetAll(
					[unload => unload.InstanceMatchID == instanceMatchID],
					withTracking: false);

				foreach (ToUnload toUnload in toUnloads)
				{
					string SANFile = Shooting.GetImagePathFromRoot(
						toUnload.CycleRID,
						toUnload.StationID,
						_imagesPath,
						anodeType,
						cameraID,
						_extension).FullName;

					int dropResponse = DLLVisionImport.fcx_drop_anode(
						(long)DataSets.TodoToDataSetID(new ToDoSimple(cameraID, anodeType)),
						SANFile);

					if (dropResponse != 0)
					{
						_logger.LogError(
							"Failed to unload anode with response code {responseCode}.",
							dropResponse);
						continue;
					}

					await _anodeUOW.Dataset.ExecuteDeleteAsync(
						dataset => dataset.SANfile == SANFile
							&& dataset.CameraID == cameraID
							&& dataset.AnodeType == anodeType);

					await _anodeUOW.ToUnload.ExecuteDeleteAsync(
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