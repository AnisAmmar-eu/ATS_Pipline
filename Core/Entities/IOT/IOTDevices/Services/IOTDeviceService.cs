using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTDevices.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Services;

public class IOTDeviceService : ServiceBaseEntity<IIOTDeviceRepository, IOTDevice, DTOIOTDevice>, IIOTDeviceService
{
	public IOTDeviceService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task CheckAllConnectionsAndUpdateTags()
	{
		List<IOTDevice> devices = await CheckAllConnections();
	}

	/// <summary>
	///		Will verify the connection on each device and will return the ones which are connected.
	/// </summary>
	/// <returns>
	///		IOTDevice with IsConnected set as True.
	/// </returns>
	private async Task<List<IOTDevice>> CheckAllConnections()
	{
		List<IOTDevice> devices = await AnodeUOW.IOTDevice.GetAll(withTracking: false, includes: "IOTTags");
		List<IOTDevice> connectedDevices = new();
		await AnodeUOW.StartTransaction();
		IEnumerable<Task> tasks = devices.Select(async device =>
		{
			device.IsConnected = await device.CheckConnection();
			if (device.IsConnected)
				connectedDevices.Add(device);
			AnodeUOW.IOTDevice.Update(device);
			AnodeUOW.Commit();
		});
		
		await Task.WhenAll(tasks);
		await AnodeUOW.CommitTransaction();
		return connectedDevices;
	}
}