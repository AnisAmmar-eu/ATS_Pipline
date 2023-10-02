using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;

public partial class OutFurnace : Furnace, IBaseEntity<OutFurnace, DTOOutFurnace>
{
	public OutFurnace(OutFurnaceStruct adsStruct)
	{
		// TODO
		// CycleStationRID = adsStruct.CycleStationRID
		// TSUnpackPIT = adsStruct.TSUnpackPIT;
		// TSCentralConveyor = adsStruct.TSCentralConveyor;
		FTAPickUp = (int)adsStruct.FTAPickUp;
	}

	public override DTOOutFurnace ToDTO(string? languageRID = null)
	{
		return new DTOOutFurnace(this);
	}
}