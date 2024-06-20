using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;
using Core.Entities.Vision.ToDos.Models.DB.Datasets;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Mapster;
using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;

namespace Core.Shared.Services.Background.Vision;

public class ReinitService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<ReinitService> _logger;
	private readonly IConfiguration _configuration;

	public ReinitService(
		ILogger<ReinitService> logger,
		IServiceScopeFactory factory,
		IConfiguration configuration)
	{
		_logger = logger;
		_factory = factory;
		_configuration = configuration;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		int cycleMS = _configuration.GetValueWithThrow<int>(ConfigDictionary.CycleMS);
		int reinitChunk = _configuration.GetValueWithThrow<int>(ConfigDictionary.ReinitChunk);

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromMilliseconds(cycleMS), stoppingToken);
			await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
			IAnodeUOW anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();

			try
			{
				ServerRule? serverRule = (ServerRule?)await anodeUOW.IOTDevice.GetBy([device => device is ServerRule]);
				if (serverRule?.Reinit != true)
					continue;

				if (await anodeUOW.ToUnload.Any())
					continue;

				while (await anodeUOW.ToMatch.Any() || await anodeUOW.Dataset.Any() || await anodeUOW.ToLoad.Any())
				{
					List<ToMatch> matches = await anodeUOW.ToMatch.GetAll(maxCount: reinitChunk);
					List<Dataset> datasets = await anodeUOW.Dataset.GetAll(maxCount: reinitChunk);
					List<ToLoad> loads = await anodeUOW.ToLoad.GetAll(maxCount: reinitChunk);

					foreach (ToMatch data in matches)
					{
						for (int i = 1; i < 2; i++)
						{
							ToSign s = data.Adapt<ToSign>();
							s.CameraID = i;
							anodeUOW.ToSign.Add(s);
						}
					}

					anodeUOW.ToMatch.RemoveRange(matches);
					anodeUOW.Commit();

					anodeUOW.ToSign.AddRange(loads.DistinctBy(x => new { x.CycleRID, x.CameraID })
						.ToList()
						.ConvertAll(x => x.Adapt<ToSign>()));
					anodeUOW.ToLoad.RemoveRange(loads);
					anodeUOW.Commit();

					anodeUOW.ToSign.AddRange(datasets.DistinctBy(x => new { x.CycleRID, x.CameraID })
						.ToList()
						.ConvertAll(x => x.Adapt<ToSign>()));
					anodeUOW.ToUnload.AddRange(datasets.ConvertAll(x => x.Adapt<ToUnload>()));
					anodeUOW.Dataset.RemoveRange(datasets);
					anodeUOW.Commit();
				}

				serverRule.Reinit = false;
				anodeUOW.IOTDevice.Update(serverRule);
				anodeUOW.Commit();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
		}
	}
}