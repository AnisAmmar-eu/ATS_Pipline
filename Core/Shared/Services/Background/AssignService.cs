using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Services;
using Core.Entities.StationCycles.Services;
using Core.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Background;

public class AssignService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<AssignService> _logger;
	private readonly TimeSpan _period = TimeSpan.FromMilliseconds(100);
	private int _executionCount;

	public AssignService(ILogger<AssignService> logger, IServiceScopeFactory factory)
	{
		_logger = logger;
		_factory = factory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
		IPacketService packetService = asyncScope.ServiceProvider.GetRequiredService<IPacketService>();
		IStationCycleService stationCycleService =
			asyncScope.ServiceProvider.GetRequiredService<IStationCycleService>();
		IConfiguration configuration = asyncScope.ServiceProvider.GetRequiredService<IConfiguration>();
		using PeriodicTimer timer = new(_period);
		string imagesPath = configuration.GetValue<string>("CameraConfig:ImagesPath");
		string thumbnailsPath = configuration.GetValue<string>("CameraConfig:ThumbnailsPath");
		while (!stoppingToken.IsCancellationRequested
		       && await timer.WaitForNextTickAsync(stoppingToken))
			try
			{
				_logger.LogInformation("AssignService running at: {time}", DateTimeOffset.Now);

				_logger.LogInformation("AssignService calling Assign");
				Packet shooting = new Shooting(imagesPath, thumbnailsPath);
				await packetService.BuildPacket(shooting);
				_logger.LogInformation("AssignService assigned shooting packet to AnodeRID: {anodeRID}",
					shooting.StationCycleRID);

				_logger.LogInformation("AssignService calling UpdateDetection");
				if (shooting.StationCycle == null)
					throw new Exception("Shooting packet did not find a stationCycle for RID: " +
					                    shooting.StationCycleRID);
				await stationCycleService.UpdateDetectionWithMeasure(shooting.StationCycle);

				_logger.LogInformation("AssignService calling AlarmList");
				Packet alarmList = new AlarmList();
				alarmList.StationCycleRID = shooting.StationCycleRID;
				alarmList.StationCycle = shooting.StationCycle;
				await packetService.BuildPacket(alarmList);
				_logger.LogInformation("AssignService assigned alarmList packet to AnodeRID: {anodeRID}",
					alarmList.StationCycleRID);

				_executionCount++;
				_logger.LogInformation(
					"Executed PeriodicAssignService - Count: {count}", _executionCount);
			}
			catch (Exception ex)
			{
				_logger.LogInformation(
					"Failed to execute PeriodicAssignService with exception message {message}. Good luck next round!",
					ex.Message);
			}
	}
}