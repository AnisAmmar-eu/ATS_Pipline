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
		StationCycleRID = adsStruct.CycleRID.ToRID();
		OutAnnounceID = adsStruct.MD.AnnounceID.ToRID();
		FTAPickUp = adsStruct.MD.PitFTA;
		PickUpTS = adsStruct.MD.PitPickup.GetTimestamp();
		DepositTS = adsStruct.MD.PitDeposit.GetTimestamp();
		InvalidPacket = adsStruct.MD.InvalidPacket;
	}

	public override DTOOutFurnace ToDTO()
	{
		return new(this);
	}
}