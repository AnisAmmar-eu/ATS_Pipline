using System.Globalization;
using System.Text;
using Core.Entities.IOT.Dictionaries;
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

	public override async Task<bool> CheckConnection()
	{
		try
		{
			Device device = CameraConnectionManager.Connect(int.Parse(Address));
			if (device.ConnectionState != ConnectionState.Connected)
				return false;
			NodeMap nodeMap = device.NodeMaps[NodeMapNames.Device];
			double temperature = (nodeMap["DeviceTemperature"] as FloatNode)!.Value;
			using HttpClient httpClient = new();
			int cameraNb = RID == DeviceRID.Camera1 ? 1 : 2;
			await httpClient.PutAsync(
				$"{ITApisDict.IOTAddress}/apiIOT/iotTags/{IOTTagRID.Temperature}{cameraNb}/{temperature}", null);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public override Task<List<IOTTag>> ApplyTags(IAnodeUOW anodeUOW)
	{
		List<IOTTag> updatedTags = new();
		Device device = CameraConnectionManager.Connect(int.Parse(Address));
		NodeMap nodeMap = device.NodeMaps[NodeMapNames.Device];
		foreach (IOTTag tag in IOTTags)
		{
			bool hasBeenUpdated = false;
			if (tag is { IsReadOnly: false, HasNewValue: true } && nodeMap[tag.Path].IsWritable)
			{
				switch (nodeMap[tag.Path])
				{
					case IntegerNode integerNode:
						integerNode.Value = int.Parse(tag.NewValue);
						break;
					case FloatNode floatNode:
						floatNode.Value = double.Parse(tag.NewValue, CultureInfo.InvariantCulture);
						break;
					case BooleanNode booleanNode:
						booleanNode.Value = bool.Parse(tag.NewValue);
						break;
					case EnumerationNode enumerationNode:
						enumerationNode.Value = tag.NewValue;
						break;
					default:
						throw new InvalidOperationException("Camera tag with path " + tag.Path +
						                                    " has a path towards unsupported data type.");
				}

				tag.HasNewValue = false;
				hasBeenUpdated = true;
			}
			else if (!tag.IsReadOnly && !nodeMap[tag.Path].IsWritable)
			{
				tag.IsReadOnly = true;
				hasBeenUpdated = true;
			}

			string readValue = nodeMap[tag.Path] switch
			{
				IntegerNode integerNode => integerNode.Value.ToString(),
				FloatNode floatNode => floatNode.Value.ToString(CultureInfo.InvariantCulture),
				BooleanNode booleanNode => booleanNode.Value.ToString(),
				EnumerationNode enumerationNode => enumerationNode.Value,
				_ => throw new InvalidOperationException("Camera tag with path " + tag.Path +
				                                         " has a path towards unsupported data type.")
			};
			hasBeenUpdated = hasBeenUpdated || tag.CurrentValue != readValue;
			if (hasBeenUpdated)
				updatedTags.Add(tag);
			tag.CurrentValue = readValue;
		}

		return Task.FromResult(updatedTags);
	}
}