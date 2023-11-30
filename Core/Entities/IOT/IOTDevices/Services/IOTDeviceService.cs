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

public class IOTDeviceService : BaseEntityService<IIOTDeviceRepository, IOTDevice, DTOIOTDevice>, IIOTDeviceService
{
	private readonly IHubContext<IOTHub, IIOTHub> _hubContext;

	public IOTDeviceService(IAnodeUOW anodeUOW, IHubContext<IOTHub, IIOTHub> hubContext) : base(anodeUOW)
	{
		_hubContext = hubContext;
	}

	public async Task<IOTDeviceStatus> GetStatusByRID(string rid)
	{
		return new IOTDeviceStatus(await AnodeUOW.IOTDevice
			.GetBy( new Expression<Func<IOTDevice, bool>>[] { device => device.RID == rid }, withTracking: false));
	}

	public async Task<List<IOTDeviceStatus>> GetStatusByArrayRID(IEnumerable<string> rids)
	{
		return (await AnodeUOW.IOTDevice.GetAll(
			new Expression<Func<IOTDevice, bool>>[] { device => rids.Contains(device.RID) },
			withTracking: false))
			.ConvertAll(device => new IOTDeviceStatus(device));
	}

	public async Task CheckAllConnectionsAndApplyTags(string[] rids)
	{
		List<IOTDevice> devices = await CheckAllConnections(rids);
		bool hasAnyTagChanged = false;

		await _hubContext.Clients.All.RefreshDevices();

		IEnumerable<Task<List<IOTTag>>> tasks = devices.Select(async device =>
		{
			List<IOTTag> updatedTags = await device.ApplyTags(AnodeUOW);
			hasAnyTagChanged = hasAnyTagChanged || updatedTags.Count != 0;
			return updatedTags;
		});
		List<IOTTag>[] updatedTags = await Task.WhenAll(tasks);
		if (hasAnyTagChanged)
		{
			await AnodeUOW.StartTransaction();
			foreach (List<IOTTag> updatedTagList in updatedTags)
				AnodeUOW.IOTTag.UpdateArray(updatedTagList.ToArray());

			AnodeUOW.Commit();
			await AnodeUOW.CommitTransaction();
			await _hubContext.Clients.All.RefreshIOTTag();
		}

		devices.ForEach(device => AnodeUOW.IOTDevice.StopTracking(device));
	}

	/// <summary>
	///     Will verify the connection on each device and will return the ones which are connected.
	/// </summary>
	/// <param name="rids"></param>
	/// <returns>
	///     IOTDevice with IsConnected set as True.
	/// </returns>
	private async Task<List<IOTDevice>> CheckAllConnections(string[] rids)
	{
		List<IOTDevice> devices = await AnodeUOW.IOTDevice.GetAll(
			new Expression<Func<IOTDevice, bool>>[] { device => rids.Contains(device.RID) },
			withTracking: true,
			includes: nameof(IOTDevice.IOTTags));
        List<IOTDevice> connectedDevices = new();
        List<IOTDevice> disconnectedDevices = new();
		IEnumerable<Task<IOTDevice>> task = devices.Select(async device =>
		{
			device.IsConnected = await device.CheckConnection();
			if (device.IsConnected)
				connectedDevices.Add(device);
			else
				disconnectedDevices.Add(device);

			return device;
		});

		IOTDevice[] updatedDevices = await Task.WhenAll(task);
		disconnectedDevices.ForEach(device => AnodeUOW.IOTDevice.StopTracking(device));
		if (updatedDevices.Length == 0)
			return connectedDevices;

		await AnodeUOW.StartTransaction();
		AnodeUOW.IOTDevice.UpdateArray(updatedDevices);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return connectedDevices;
	}
}