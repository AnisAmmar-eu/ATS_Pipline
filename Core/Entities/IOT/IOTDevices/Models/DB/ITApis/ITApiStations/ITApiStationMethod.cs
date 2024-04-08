using System.Net;
using Core.Entities.IOT.IOTDevices.Models.DTO.ITApiStations;
using Microsoft.Extensions.Logging;

namespace Core.Entities.IOT.IOTDevices.Models.DB.ITApiStations;

public partial class ITApiStation
{
	public ITApiStation()
	{
	}

	public ITApiStation(DTOITApiStation dtoITApiStation) : base(dtoITApiStation)
	{
	}

	public override DTOITApiStation ToDTO()
	{
		return new(this);
	}

	public override async Task<bool> CheckConnection(ILogger logger)
	{
		using HttpClient httpClient = new();
		CancellationTokenSource cancelSource = new();
		bool isConnected;
		try
		{
			HttpResponseMessage response = await httpClient.GetAsync(Address + ConnectionPath);
			isConnected = response.StatusCode == HttpStatusCode.OK;
		}
		catch (Exception e)
		{
			logger.LogInformation("Error while trying to connect to {name}:\n{error}", RID, e);
			isConnected = false;
		}

		return isConnected;
	}
}