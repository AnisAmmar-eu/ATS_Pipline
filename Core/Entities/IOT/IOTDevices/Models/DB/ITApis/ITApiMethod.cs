using System.Net;
using Core.Entities.IOT.IOTDevices.Models.DTO.ITApis;
using Microsoft.Extensions.Logging;

namespace Core.Entities.IOT.IOTDevices.Models.DB.ITApis;

public partial class ITApi
{
	public ITApi()
	{
	}

	public ITApi(DTOITApi dtoITApi) : base(dtoITApi)
	{
	}

	public override DTOITApi ToDTO()
	{
		return new(this);
	}

	public override async Task<bool> CheckConnection(ILogger logger)
	{
		using HttpClient httpClient = new();
		try
		{
			HttpResponseMessage response = await httpClient.GetAsync(Address + ConnectionPath);
			return response.StatusCode == HttpStatusCode.OK;
		}
		catch (Exception e)
		{
			logger.LogInformation("Error while trying to connect to {name}:\n{error}", RID, e);
			return false;
		}
	}
}