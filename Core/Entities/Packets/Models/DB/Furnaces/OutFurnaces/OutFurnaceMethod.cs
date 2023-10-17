using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;

public partial class OutFurnace : Furnace, IBaseEntity<OutFurnace, DTOOutFurnace>
{
	public OutFurnace()
	{
	}

	public OutFurnace(OutFurnaceStruct adsStruct)
	{
		StationCycleRID = adsStruct.StationCycleRID.ToRID();
		AnnounceID = adsStruct.AnnounceID;
		FTAPickUp = adsStruct.FTAPickUp;
		PickUpTS = adsStruct.PickUpTS.GetTimestamp();
		DepositTS = adsStruct.DepositTS.GetTimestamp();
		InvalidPacket = adsStruct.InvalidPacket;
	}

	public override DTOOutFurnace ToDTO()
	{
		return new DTOOutFurnace(this);
	}
}