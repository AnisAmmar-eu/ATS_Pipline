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

	public override async Task<bool> ApplyTags(IAnodeUOW anodeUOW)
	{
		Dictionary<string, string> parameters = new();
		bool hasAnyTagChanged = false;
		foreach (IOTTag iotTag in IOTTags)
		{
			if (!iotTag.HasNewValue)
				continue;

			parameters.Add(iotTag.Path, iotTag.NewValue);
			iotTag.CurrentValue = iotTag.NewValue;
			iotTag.HasNewValue = false;
			anodeUOW.IOTTag.Update(iotTag);
		}

		if (parameters.Count != 0)
		{
			using HttpClient httpClient = new();
			try
			{
				HttpResponseMessage response =
					await httpClient.PostAsync(Address + "/set-parameters", JsonContent.Create(parameters));
				if (response.StatusCode != HttpStatusCode.OK)
					throw new Exception("Error while setting parameters: " + response.StatusCode);
				hasAnyTagChanged = true;
			}
			catch (Exception e)
			{
				Console.WriteLine("Could not update camera parameters: " + e.Message);
				return false;
			}
		}

		anodeUOW.Commit();
		return hasAnyTagChanged;
	}
}