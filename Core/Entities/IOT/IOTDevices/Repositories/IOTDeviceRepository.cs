using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.IOT.IOTDevices.Repositories;

public class IOTDeviceRepository : RepositoryBaseEntity<AnodeCTX, IOTDevice, DTOIOTDevice>, IIOTDeviceRepository
{
	public IOTDeviceRepository(AnodeCTX context) : base(context)
	{
	}

	public void StopTracking(IOTDevice device)
	{
		Context.Entry(device).State = EntityState.Detached;
	}
}