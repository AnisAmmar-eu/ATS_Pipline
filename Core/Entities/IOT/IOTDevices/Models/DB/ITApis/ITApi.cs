using Core.Entities.IOT.IOTDevices.Models.DTO.ITApis;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB.ITApis;

public partial class ITApi : IOTDevice, IBaseEntity<ITApi, DTOITApi>
{
}