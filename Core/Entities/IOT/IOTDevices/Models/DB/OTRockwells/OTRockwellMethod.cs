using Core.Entities.IOT.IOTDevices.Models.DTO.OTRockwells;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DB.OTRockwells;

public partial class OTRockwell : IOTDevice, IBaseEntity<OTRockwell, DTOOTRockwell>
{
	public override DTOOTRockwell ToDTO()
	{
		return new DTOOTRockwell(this);
	}
}