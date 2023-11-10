using Core.Entities.IOT.IOTDevices.Models.DB;

namespace Core.Entities.IOT.IOTDevices.Models.Structs;

public struct IOTDeviceStatus
{
	public string RID { get; set; } = string.Empty;
	public bool IsConnected { get; set; } = false;

	public IOTDeviceStatus(IOTDevice device)
	{
		RID = device.RID;
		IsConnected = device.IsConnected;
	}
}