using System.Globalization;
using Core.Entities.IOT.IOTDevices.Models.DTO.OTCameras;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Models.Camera;
using Core.Shared.Models.DB.Kernel.Interfaces;
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

	public override Task<bool> CheckConnection()
	{
		try
		{
			Device device = CameraConnectionManager.Connect(int.Parse(Address));
			if (device.ConnectionState != ConnectionState.Connected)
				return Task.FromResult(false);
			NodeMap nodeMap = device.NodeMaps[NodeMapNames.Device];
			Temperature = (nodeMap["DeviceTemperature"] as FloatNode)!.Value;
			return Task.FromResult(true);
		}
		catch (Exception)
		{
			return Task.FromResult(false);
		}
	}

	public override Task<List<IOTTag>> ApplyTags(IAnodeUOW anodeUOW)
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
			Device device = CameraConnectionManager.Connect(int.Parse(Address));
			NodeMap nodeMap = device.NodeMaps[NodeMapNames.Device];
			foreach ((string? path, string? newValue) in parameters)
				if (nodeMap[path].IsWritable)
					switch (nodeMap[path])
					{
						case IntegerNode { IsWritable: true } integerNode:
							integerNode.Value = int.Parse(newValue);
							break;
						case FloatNode { IsWritable: true } floatNode:
							floatNode.Value = double.Parse(newValue, CultureInfo.InvariantCulture);
							break;
						case EnumerationNode { IsWritable: true } enumerationNode:
							enumerationNode.Value = newValue;
							break;
						default:
							throw new InvalidOperationException("Camera tag with path " + path +
							                                    " has a path towards unsupported data type.");
					}
		}

		return Task.FromResult(updatedTags);
	}
}