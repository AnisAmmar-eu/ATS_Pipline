using System.Net;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DTO.ITApis;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB.ITApis;

public partial class ITApi : IOTDevice, IBaseEntity<ITApi, DTOITApi>
{
	public override DTOITApi ToDTO()
	{
		return new DTOITApi(this);
	}

	public override async Task<bool> CheckConnection()
	{
		using HttpClient httpClient = new();
		IOTTag tag = IOTTags.Find(tag => tag.Name == IOTTagNames.CheckConnectionName)
		             ?? throw new InvalidOperationException("Cannot find Connection tag for " + Name + " device.");
		try
		{
			HttpResponseMessage response = await httpClient.GetAsync(Address + tag.Path);
			return response.StatusCode == HttpStatusCode.OK;
		}
		catch (Exception)
		{
			return false;
		}
	}
}