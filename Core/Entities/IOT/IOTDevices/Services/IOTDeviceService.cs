using System.Linq.Expressions;
using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTDevices.Repositories;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.Parameters.CameraParams.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Stemmer.Cvb;

namespace Core.Entities.IOT.IOTDevices.Services;

public class IOTDeviceService : ServiceBaseEntity<IIOTDeviceRepository, IOTDevice, DTOIOTDevice>, IIOTDeviceService
{
	public IOTDeviceService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}


	public async Task<List<DTOIOTDevice>> GetAllWithIncludes()
	{
		return (await AnodeUOW.IOTDevice.GetAll(withTracking: false, includes: "IOTTags")).ConvertAll(device =>
			device.ToDTO());
	}
	public async Task<DTOIOTDevice> GetByRIDWithIncludes(string rid)
	{
		return (await AnodeUOW.IOTDevice.GetBy(filters: new Expression<Func<IOTDevice, bool>>[]
		{
			device => device.RID == rid
		}, withTracking: false, includes: new []{"IOTTags"})).ToDTO();
	}

	public async Task CheckAllConnectionsAndApplyTags()
	{
		List<IOTDevice> devices = await CheckAllConnections();
		await AnodeUOW.StartTransaction();
		IEnumerable<Task> tasks = devices.Select(async device => await device.ApplyTags(AnodeUOW));
		await Task.WhenAll(tasks);
		await AnodeUOW.CommitTransaction();
	}

	/// <summary>
	///		Will verify the connection on each device and will return the ones which are connected.
	/// </summary>
	/// <returns>
	///		IOTDevice with IsConnected set as True.
	/// </returns>
	private async Task<List<IOTDevice>> CheckAllConnections()
	{
		string cameraDeviceString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\GenICam.vin";
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