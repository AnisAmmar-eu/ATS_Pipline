using Core.Entities.IOT.IOTDevices.Models.DB.OTRockwells;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.OTRockwells;

public partial class DTOOTRockwell : DTOIOTDevice, IDTO<OTRockwell, DTOOTRockwell>
{
	public DTOOTRockwell() : base()
	{
	}

	public DTOOTRockwell(OTRockwell otRockwell) : base(otRockwell)
	{
	}
}