using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTDevices.Models.Structs;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Services;

public interface IIOTDeviceService : IBaseEntityService<IOTDevice, DTOIOTDevice>
{
	public Task<IOTDeviceStatus> GetStatusByRID(string rid);
	public Task<List<IOTDeviceStatus>> GetStatusByArrayRID(IEnumerable<string> rids);
	public Task<IEnumerable<string>> GetAllApisRIDs();

	/// <summary>
	/// Will verify the connection on every device given. If connected, devices' tags are then applied.
	/// Both devices and tags are updated.
	/// </summary>
	/// <param name="rids">RIDs of all devices to monitor and apply tags to.</param>
	/// <returns></returns>
	public Task CheckAllConnectionsAndApplyTags(IEnumerable<string> rids);

	/// <summary>
	/// /// Will reinitialize all devices in the active mode.
	/// </summary>
	/// <returns></returns>
	public Task ActiveReinit();
}