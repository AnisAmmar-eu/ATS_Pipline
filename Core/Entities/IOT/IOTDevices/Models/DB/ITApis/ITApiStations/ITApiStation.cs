using Core.Entities.IOT.IOTDevices.Models.DB.ITApis;
using Core.Entities.IOT.IOTDevices.Models.DTO.ITApiStations;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB.ITApiStations;

public partial class ITApiStation : ITApi, IBaseEntity<ITApiStation, DTOITApiStation>
{
	public DateTimeOffset OldestTSShooting { get; set; }
	public bool IsOptional { get; set; }
}