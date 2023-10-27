using Core.Entities.IOT.IOTDevices.Models.DTO.OTCameras;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;

public partial class OTCamera : IOTDevice, IBaseEntity<OTCamera, DTOOTCamera>
{
	public double Temperature { get; set; }
}