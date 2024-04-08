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
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Mapster;
using Core.Entities.Vision.ToDos.Models.DB.Datasets;

namespace Core.Shared.Services.Background.Vision;

public class LoadService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<LoadService> _logger;
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

	/// <summary>Load in dataset</summary>
	/// <summary>Need to get all ToLoad with InstanceMatchID</summary>
	/// <summary>Then use DLLVision to load</summary>
	/// <summary>Update dataset having add loaded anode line</summary>
	/// <summary>Then delete load line from Load table</summary>
	/// <param name="stoppingToken">Cancellation token</param>
	/// <returns>Task</returns>
	/// <exception cref="Exception">Failed to execute LoadService with exception message {message}.</exception>
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
				List<ToLoad> toLoads = await _anodeUOW.ToLoad.GetAll(
					[load => load.InstanceMatchID == instanceMatchID],
					withTracking: false);

				foreach (ToLoad toLoad in toLoads)
				{
					string SANFile = Shooting.GetImagePathFromRoot(
						toLoad.CycleRID,
						toLoad.StationID,
						_imagesPath,
						anodeType,
						cameraID,
						_extension).FullName;

					int loadResponse = DLLVisionImport.fcx_load_anode(
						(long)DataSets.TodoToDataSetID(new ToDoSimple(cameraID, anodeType)),
						SANFile,
						toLoad.CycleRID);

					if (loadResponse != 0)
					{
						_logger.LogError(
							"Failed to unload anode with response code {responseCode}.",
							loadResponse);
						continue;
					}

					Dataset dataset = toLoad.Adapt<Dataset>();
					dataset.SANfile = SANFile;
					await _anodeUOW.Dataset.Add(dataset);
					_anodeUOW.Commit();

					await _anodeUOW.ToLoad.ExecuteDeleteAsync(
						load => load.ID == toLoad.ID);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute LoadService with exception message {message}.",
					ex.Message);
			}
		}
	}
}