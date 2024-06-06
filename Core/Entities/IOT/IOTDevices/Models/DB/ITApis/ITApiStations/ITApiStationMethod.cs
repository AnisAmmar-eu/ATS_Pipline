using System.Net;
using System.Text.Json;
using Core.Entities.IOT.IOTDevices.Models.DTO.ITApiStations;
using Core.Shared.Models.ApiResponses;
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

	public override DTOITApiStation ToDTO() => new(this);

	public override async Task<bool> CheckConnection(ILogger logger)
	{
		using HttpClient httpClient = new();
		CancellationTokenSource cancelSource = new();
		bool isConnected;

		if (IsOptional)
		{
			isConnected = true;
			OldestTSShooting = DateTimeOffset.MinValue;
		}
		else
		{
			try
			{
				HttpResponseMessage response = await httpClient.GetAsync(Address + ConnectionPath);
				isConnected = response.StatusCode == HttpStatusCode.OK;

				ApiResponse apiResponse
				= JsonSerializer.Deserialize<ApiResponse>(await response.Content.ReadAsStreamAsync())
					?? throw new ApplicationException("Could not deserialize ApiIOT response");
				if (apiResponse.Result is not JsonElement jsonElement)
					throw new ApplicationException("JSON Exception, ApiResponse from ApiIOT is broken");

				OldestTSShooting = jsonElement.Deserialize<DateTimeOffset>();
			}
			catch (Exception e)
			{
				logger.LogInformation("Error while trying to connect to {name}:\n{error}", RID, e);
				isConnected = false;
				OldestTSShooting = DateTimeOffset.MinValue;
			}
		}

		return isConnected;
	}
}