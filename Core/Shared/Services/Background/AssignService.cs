using System.Drawing.Printing;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.Packets.Services;
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
		using PeriodicTimer timer = new(_period);

		while (!stoppingToken.IsCancellationRequested
		       && await timer.WaitForNextTickAsync(stoppingToken))
			try
			{
				await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
				IPacketService packetService = asyncScope.ServiceProvider.GetRequiredService<IPacketService>();

				_logger.LogInformation("AssignService running at: {time}", DateTimeOffset.Now);
				_logger.LogInformation("AssignService calling Assign");
				DTOPacket dtoShooting = new DTOShooting();
				dtoShooting = await packetService.BuildPacket(dtoShooting);
				_logger.LogInformation("AssignService assigned shooting packet to AnodeRID: {anodeRID}", dtoShooting.StationCycleRID);

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