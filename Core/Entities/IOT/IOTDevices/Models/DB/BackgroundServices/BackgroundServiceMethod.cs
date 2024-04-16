using Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices;
using Core.Shared.Dictionaries;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices;

public partial class BackgroundService
{
	public BackgroundService()
	{
	}

	public override DTOBackgroundService ToDTO() => this.Adapt<DTOBackgroundService>();

	public override async Task<bool> CheckConnection(ILogger logger)
	{
		bool isConnected;
		try
		{
			isConnected = this.WatchdogTime.Add(Server.WatchdogDelay) > DateTimeOffset.Now;
		}
		catch (Exception e)
		{
			logger.LogInformation("Error while trying to connect to {name}:\n{error}", RID, e);
			isConnected = false;
		}

		return isConnected;
	}
}