using System.Globalization;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DTO.OTCameras;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Dictionaries;
using Core.Shared.Models.Camera;
using Core.Shared.Models.TwinCat;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;
using Stemmer.Cvb;
using Stemmer.Cvb.Driver;
using Stemmer.Cvb.GenApi;
using TwinCAT.Ads;

namespace Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;

public partial class OTCamera
{
	public OTCamera()
	{
	}

	public OTCamera(DTOOTCamera dtoOTCamera) : base(dtoOTCamera)
	{
	}

	public override DTOOTCamera ToDTO()
	{
		return new(this);
	}

	public override async Task<bool> CheckConnection(ILogger logger)
	{
		bool isConnected;
		CancellationTokenSource cancelSource = new();
		cancelSource.CancelAfter(400);
		try
		{
			Device device = await CameraConnectionManager.Connect(int.Parse(Address), cancelSource.Token);
			if (device.ConnectionState != ConnectionState.Connected)
				return false;

			NodeMap nodeMap = device.NodeMaps[NodeMapNames.Device];
			double temperature = (nodeMap["DeviceTemperature"] as FloatNode)!.Value;
			using HttpClient httpClient = new();
			string tempRID = (RID == DeviceRID.Camera1) ? IOTTagRID.TemperatureCam1 : IOTTagRID.TemperatureCam2;
			await httpClient.PutAsync(
				$"{ITApisDict.IOTAddress}/apiIOT/iotTags/{tempRID}/{temperature.ToString(CultureInfo.InvariantCulture)}", null);
			isConnected = true;
		}
		catch (Exception e)
		{
			logger.LogInformation("Error while monitoring to {camRID}:\n {error}", RID, e);
			isConnected = false;
		}

		try
		{
			AdsClient tcClient = await TwinCatConnectionManager.Connect(ADSUtils.AdsPort, cancelSource.Token);
			uint statusHandle
				= tcClient.CreateVariableHandle((RID == DeviceRID.Camera1) ? IOTTagPath.Cam1Status : IOTTagPath.Cam2Status);
			tcClient.WriteAny(statusHandle, isConnected);
		}
		catch (Exception e)
		{
			logger.LogInformation("Error while writing {camRID} status in TwinCat:\n {error}", RID, e);
		}

		return isConnected;
	}

	public override async Task<List<IOTTag>> ApplyTags(IAnodeUOW anodeUOW)
	{
		List<IOTTag> updatedTags = [];
        Device device;
		CancellationTokenSource cancelSource = new();
		cancelSource.CancelAfter(200);
		try
		{
			device = await CameraConnectionManager.Connect(int.Parse(Address), cancelSource.Token);
		}
		catch (IOException)
		{
			return [];
		}

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
						throw new InvalidOperationException(
							$"Camera tag with path {tag.Path} has a path towards unsupported data type.");
				}

				tag.HasNewValue = false;
				hasBeenUpdated = true;
			}
			else if (!tag.IsReadOnly && !nodeMap[tag.Path].IsWritable)
			{
				tag.IsReadOnly = true;
				hasBeenUpdated = true;
			}

			string readValue = nodeMap[tag.Path] switch {
				IntegerNode integerNode => integerNode.Value.ToString(),
				FloatNode floatNode => floatNode.Value.ToString(CultureInfo.InvariantCulture),
				BooleanNode booleanNode => booleanNode.Value.ToString(),
				EnumerationNode enumerationNode => enumerationNode.Value,
				_ => throw new InvalidOperationException(
					$"Camera tag with path {tag.Path} has a path towards unsupported data type."),
			};
			hasBeenUpdated = hasBeenUpdated || tag.CurrentValue != readValue || tag.CurrentValue != tag.NewValue;
			if (hasBeenUpdated)
				updatedTags.Add(tag);

			tag.CurrentValue = readValue;
			if (!tag.HasNewValue)
				tag.NewValue = readValue;
		}

		return updatedTags;
	}
}