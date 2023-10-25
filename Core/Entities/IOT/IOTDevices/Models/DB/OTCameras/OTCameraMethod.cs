using System.Net;
using System.Net.Http.Json;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DTO.OTCameras;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;

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

	public override async Task<List<IOTTag>> ApplyTags(IAnodeUOW anodeUOW)
	{
		Dictionary<string, string> parameters = new();
		List<IOTTag> updatedTags = new();
		string httpPath = string.Empty;
		foreach (IOTTag iotTag in IOTTags)
		{
			if (httpPath == string.Empty && iotTag.Name == IOTTagNames.CheckConnectionName)
				httpPath = iotTag.Path;
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
					await httpClient.PostAsync(Address + httpPath, JsonContent.Create(parameters));
				if (response.StatusCode != HttpStatusCode.OK)
					throw new Exception("Error while setting parameters: " + response.StatusCode);
			}
			catch (Exception e)
			{
				Console.WriteLine("Could not update camera parameters: " + e.Message);
				return new List<IOTTag>();
			}
		}

		anodeUOW.Commit();
		return updatedTags;
	}
}