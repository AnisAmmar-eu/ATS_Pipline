using System.Linq.Expressions;
using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTDevices.Models.Structs;
using Core.Entities.IOT.IOTDevices.Repositories;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Services.Kernel;
using Core.Shared.SignalR.IOTHub;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Core.Entities.IOT.IOTDevices.Services;

public class IOTDeviceService : ServiceBaseEntity<IIOTDeviceRepository, IOTDevice, DTOIOTDevice>, IIOTDeviceService
{
	private readonly IHubContext<IOTHub, IIOTHub> _hubContext;

	public IOTDeviceService(IAnodeUOW anodeUOW, IHubContext<IOTHub, IIOTHub> hubContext) : base(anodeUOW)
	{
		_hubContext = hubContext;
	}

	public async Task<IOTDeviceStatus> GetStatusByRID(string rid)
	{
		return new IOTDeviceStatus(await AnodeUOW.IOTDevice.GetBy(new Expression<Func<IOTDevice, bool>>[]
		{
			device => device.RID == rid
		}, withTracking: false));
	}

	public async Task<List<IOTDeviceStatus>> GetStatusByArrayRID(IEnumerable<string> rids)
	{
		return (await AnodeUOW.IOTDevice.GetAll(filters: new Expression<Func<IOTDevice, bool>>[]
		{
			device => rids.Contains(device.RID)
		}, withTracking: false)).ConvertAll(device => new IOTDeviceStatus(device));
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
		bool hasAnyTagChanged = false;
		for (int i = 0; i < devices.Count; i++)
		{
			AnodeUOW.IOTDevice.StopTracking(devices[i]);
			devices[i] = await AnodeUOW.IOTDevice.GetById(devices[i].ID, withTracking: false, includes: "IOTTags");
		}

		await _hubContext.Clients.All.RefreshDevices();

		IEnumerable<Task<List<IOTTag>>> tasks = devices.Select(async device =>
		{
			List<IOTTag> updatedTags = await device.ApplyTags(AnodeUOW);
			hasAnyTagChanged = hasAnyTagChanged || updatedTags.Any();
			return updatedTags;
		});
		if (!hasAnyTagChanged)
			return;
		await AnodeUOW.StartTransaction();
		List<IOTTag>[] updatedTags = await Task.WhenAll(tasks);
		foreach (List<IOTTag> updatedTagList in updatedTags)
			AnodeUOW.IOTTag.UpdateArray(updatedTagList.ToArray());

		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
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
		List<IOTDevice> devices = await AnodeUOW.IOTDevice.GetAll(withTracking: false);
		List<IOTDevice> connectedDevices = new();
		IEnumerable<Task<IOTDevice>> task = devices.Select(async device =>
		{
			device.IsConnected = await device.CheckConnection();
			if (device.IsConnected)
				connectedDevices.Add(device);
			return device;
		});

		IOTDevice[] updatedDevices = await Task.WhenAll(task);
		if (!updatedDevices.Any()) return connectedDevices;

		await AnodeUOW.StartTransaction();
		AnodeUOW.IOTDevice.UpdateArray(updatedDevices);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return connectedDevices;
	}
}