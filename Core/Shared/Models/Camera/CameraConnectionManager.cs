using Stemmer.Cvb;

namespace Core.Shared.Models.Camera;

public static class CameraConnectionManager
{
	/// <summary>
	///     Could be a list but by being a dictionary, having 2 or 42 cameras does not matter as there is no need to specify
	///     a maximum length for the list.
	/// </summary>
	private static Dictionary<int, Device> Devices { get; } = new();

	public static async Task<Device> Connect(int port, CancellationToken cancel)
	{
		if (Devices.TryGetValue(port, out Device? existingDevice))
			return existingDevice;

		return await Task.Run(
			() =>
			{
				while (!cancel.IsCancellationRequested)
				{
					try
					{
						string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\GenICam.vin";
						Device device = DeviceFactory.OpenPort(driverString, port);
						Devices.Add(port, device);
						return device;
					}
					catch
					{
						// ignored
					}
				}

				throw new IOException($"Could not connect to camera with port: {port.ToString()}");
			},
			cancel);
	}
}