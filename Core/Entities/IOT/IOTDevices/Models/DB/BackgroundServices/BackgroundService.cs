using Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices;

public partial class BackgroundService: IOTDevice, IBaseEntity<BackgroundService, DTOBackgroundService>
{
	public int StationID { get; set; }
	public string AnodeType { get; set; }
	public DateTimeOffset WatchdogTime { get; set; } = DateTimeOffset.MinValue;
	public bool Pause { get; set; }
}