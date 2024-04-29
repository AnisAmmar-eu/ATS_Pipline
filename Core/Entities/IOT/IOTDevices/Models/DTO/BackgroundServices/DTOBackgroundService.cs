using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices;

public partial class DTOBackgroundService : DTOIOTDevice, IDTO<BackgroundService, DTOBackgroundService>
{
	public int StationID { get; set; }
	public string AnodeType { get; set; }
}