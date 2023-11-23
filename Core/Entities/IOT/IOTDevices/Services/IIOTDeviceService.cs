using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Entities.IOT.IOTDevices.Models.Structs;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Services;

public interface IIOTDeviceService : IServiceBaseEntity<IOTDevice, DTOIOTDevice>
{
	public Task<IOTDeviceStatus> GetStatusByRID(string rid);
	public Task<List<IOTDeviceStatus>> GetStatusByArrayRID(IEnumerable<string> rids);
	public Task CheckAllConnectionsAndApplyTags(string[] rids);
}