using System.Net;
using Core.Entities.IOT.IOTDevices.Models.DTO.ITApis;

namespace Core.Entities.IOT.IOTDevices.Models.DB.ITApis;

public partial class ITApi
{
	public override DTOITApi ToDTO()
	{
		return new DTOITApi(this);
	}

	public override async Task<bool> CheckConnection()
	{
		using HttpClient httpClient = new();
		try
		{
			HttpResponseMessage response = await httpClient.GetAsync(Address + ConnectionPath);
			return response.StatusCode == HttpStatusCode.OK;
		}
		catch (Exception)
		{
			return false;
		}
	}
}