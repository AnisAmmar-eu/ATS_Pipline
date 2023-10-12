using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Services;

public interface IIOTDeviceService : IServiceBaseEntity<IOTDevice, DTOIOTDevice>
{
	public Task CheckAllConnectionsAndUpdateTags();
}