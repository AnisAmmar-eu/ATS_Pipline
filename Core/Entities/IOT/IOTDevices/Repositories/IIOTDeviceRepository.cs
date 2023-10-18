using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Repositories;

public interface IIOTDeviceRepository : IRepositoryBaseEntity<IOTDevice, DTOIOTDevice>
{
}