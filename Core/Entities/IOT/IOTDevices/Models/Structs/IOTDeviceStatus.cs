using Core.Entities.IOT.IOTDevices.Models.DB;

namespace Core.Entities.IOT.IOTDevices.Models.Structs;

/// <summary>
/// Lighter version of an IOTDevice for quicker status reporting.
/// </summary>
public struct IOTDeviceStatus
{
	public string RID { get; set; } = string.Empty;
	public bool IsConnected { get; set; }

	public IOTDeviceStatus(IOTDevice device)
	{
		RID = device.RID;
		IsConnected = device.IsConnected;
	}
}