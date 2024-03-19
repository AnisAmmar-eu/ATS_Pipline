using Core.Entities.IOT.IOTDevices.Models.DTO.Stations;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB.Stations;

public partial class Station : IOTDevice, IBaseEntity<Station, DTOStation>
{
    public DateTimeOffset oldestShooting { get; set; }
}