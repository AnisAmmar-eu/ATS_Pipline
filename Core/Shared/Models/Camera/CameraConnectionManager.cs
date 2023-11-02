using Stemmer.Cvb;

namespace Core.Shared.Models.Camera;

public static class CameraConnectionManager
{
	// Could be a list but by being a dictionary, having 2 or 42 cameras does not matter as there is no need to specify
	// a maximum length for the list.
	private static Dictionary<int, Device> Devices { get; set; } = new();

	public static Device Connect(int port)
	{
		if (Devices.TryGetValue(port, out Device? existingDevice))
			return existingDevice;
		
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\GenICam.vin";
		Device device = DeviceFactory.OpenPort(driverString, port);
		Devices.Add(port, device);
		return device;
	}
}