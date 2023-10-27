using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;

namespace Core.Entities.IOT.IOTDevices.Models.Structs;

public struct IOTDeviceStatus
{
	public bool IsConnected { get; set; } = false;
	public double? Temperature { get; set; } = null;

	public IOTDeviceStatus(IOTDevice device)
	{
		IsConnected = device.IsConnected;
		if (device is OTCamera camera)
			Temperature = camera.Temperature;
	}
}