using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;

public partial class DTOOutFurnace : DTOFurnace, IDTO<OutFurnace, DTOOutFurnace>
{
	public DTOOutFurnace(OutFurnace outFurnace) : base(outFurnace)
	{
		TSUnpackPIT = outFurnace.TSUnpackPIT;
		TSCentralConveyor = outFurnace.TSCentralConveyor;
		FTAPickUp = outFurnace.FTAPickUp;
	}
}