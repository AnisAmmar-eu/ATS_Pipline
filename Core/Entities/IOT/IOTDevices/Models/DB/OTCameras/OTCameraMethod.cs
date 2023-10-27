using System.Net;
using System.Net.Http.Json;
using Core.Entities.IOT.IOTDevices.Models.DTO.OTCameras;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;
using Newtonsoft.Json;

namespace Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;

public partial class OTCamera : IOTDevice, IBaseEntity<OTCamera, DTOOTCamera>
{
	public override DTOOTCamera ToDTO()
	{
		return new DTOOTCamera(this);
	}

	public override async Task<bool> CheckConnection()
	{
		using HttpClient httpClient = new();
		try
		{
			HttpResponseMessage response = await httpClient.GetAsync(Address + ConnectionPath);
			if (response.StatusCode != HttpStatusCode.OK)
				return false;
			dynamic content = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
			Temperature = content.result;
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public override async Task<List<IOTTag>> ApplyTags(IAnodeUOW anodeUOW)
	{
		Dictionary<string, string> parameters = new();
		List<IOTTag> updatedTags = new();
		foreach (IOTTag iotTag in IOTTags)
		{
			if (!iotTag.HasNewValue)
				continue;

			parameters.Add(iotTag.Path, iotTag.NewValue);
			iotTag.CurrentValue = iotTag.NewValue;
			iotTag.HasNewValue = false;
			updatedTags.Add(iotTag);
		}

		if (parameters.Count != 0)
		{
			using HttpClient httpClient = new();
			try
			{
				HttpResponseMessage response =
					await httpClient.PostAsync(Address + ConnectionPath, JsonContent.Create(parameters));
				if (response.StatusCode != HttpStatusCode.OK)
					throw new Exception("Error while setting parameters: " + response.StatusCode);
			}
			catch (Exception e)
			{
				Console.WriteLine("Could not update camera parameters: " + e.Message);
				return new List<IOTTag>();
			}
		}

		return updatedTags;
	}
}