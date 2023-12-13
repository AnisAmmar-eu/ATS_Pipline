using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;

namespace Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;

public partial class OutFurnace
{
	public OutFurnace()
	{
	}

	public OutFurnace(DTOOutFurnace dtoOutFurnace) : base(dtoOutFurnace)
	{
		OutAnnounceID = dtoOutFurnace.OutAnnounceID;
		FTAPickUp = dtoOutFurnace.FTAPickUp;
		PickUpTS = dtoOutFurnace.PickUpTS;
		DepositTS = dtoOutFurnace.DepositTS;
		InvalidPacket = dtoOutFurnace.InvalidPacket;
	}

	public OutFurnace(OutFurnaceStruct adsStruct)
	{
		StationCycleRID = adsStruct.StationCycleRID.ToRID();
		OutAnnounceID = adsStruct.AnnounceID.ToRID();
		FTAPickUp = adsStruct.FTAPickUp;
		PickUpTS = adsStruct.PickUpTS.GetTimestamp();
		DepositTS = adsStruct.DepositTS.GetTimestamp();
		InvalidPacket = adsStruct.InvalidPacket;
	}

	public override DTOOutFurnace ToDTO()
	{
		return new(this);
	}
}