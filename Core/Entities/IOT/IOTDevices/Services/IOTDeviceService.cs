using System.Linq.Expressions;
using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTDevices.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.SignalR.IOTTagHub;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Core.Entities.IOT.IOTDevices.Services;

public class IOTDeviceService : ServiceBaseEntity<IIOTDeviceRepository, IOTDevice, DTOIOTDevice>, IIOTDeviceService
{
	private readonly IHubContext<IOTTagHub, IIOTTagHub> _hubContext;

	public IOTDeviceService(IAnodeUOW anodeUOW, IHubContext<IOTTagHub, IIOTTagHub> hubContext) : base(anodeUOW)
	{
		_hubContext = hubContext;
	}


	public async Task<List<DTOIOTDevice>> GetAllWithIncludes()
	{
		return (await AnodeUOW.IOTDevice.GetAll(withTracking: false, includes: "IOTTags")).ConvertAll(device =>
			device.ToDTO());
	}

	public async Task<DTOIOTDevice> GetByRIDWithIncludes(string rid)
	{
		return (await AnodeUOW.IOTDevice.GetBy(new Expression<Func<IOTDevice, bool>>[]
		{
			device => device.RID == rid
		}, withTracking: false, includes: new[] { "IOTTags" })).ToDTO();
	}

	public async Task CheckAllConnectionsAndApplyTags()
	{
		List<IOTDevice> devices = await CheckAllConnections();
		await AnodeUOW.StartTransaction();
		bool hasAnyTagChanged = false;
		IEnumerable<Task> tasks = devices.Select(async device =>
			hasAnyTagChanged = hasAnyTagChanged || await device.ApplyTags(AnodeUOW));
		await Task.WhenAll(tasks);
		await AnodeUOW.CommitTransaction();
		if (hasAnyTagChanged)
			await _hubContext.Clients.All.RefreshIOTTag();
	}

	/// <summary>
	///     Will verify the connection on each device and will return the ones which are connected.
	/// </summary>
	/// <returns>
	///     IOTDevice with IsConnected set as True.
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