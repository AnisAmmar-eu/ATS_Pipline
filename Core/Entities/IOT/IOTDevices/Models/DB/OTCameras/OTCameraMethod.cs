using System.Net;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DTO.OTCameras;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Stemmer.Cvb;
using Stemmer.Cvb.Driver;
using Stemmer.Cvb.GenApi;

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

	public override async Task ApplyTags(IAnodeUOW anodeUOW)
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\\GenICam.vin";
		Device device = DeviceFactory.Open(driverString);
		NodeMap nodeMap = device.NodeMaps[NodeMapNames.Device];
		foreach (IOTTag iotTag in IOTTags)
		{
			if (!iotTag.HasNewValue)
				continue;
			switch (nodeMap[iotTag.Path])
			{
				case IntegerNode {IsWritable: true} integerNode:
					integerNode.Value = int.Parse(iotTag.NewValue);
					break;
				case FloatNode {IsWritable: true} floatNode:
					floatNode.Value = double.Parse(iotTag.NewValue);
					break;
				case EnumerationNode {IsWritable: true} enumerationNode:
					enumerationNode.Value = iotTag.NewValue;
					break;
				default:
					throw new InvalidOperationException("Camera tag " + iotTag.Name +
					                                    " has a path towards unsupported or unwritable data type.");
			}

			iotTag.CurrentValue = iotTag.NewValue;
			iotTag.HasNewValue = false;
			anodeUOW.IOTTag.Update(iotTag);
			anodeUOW.Commit();
		}
	}
}