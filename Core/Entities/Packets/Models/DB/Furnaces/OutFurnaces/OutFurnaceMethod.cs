using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;
using Core.Entities.Packets.Models.Structs;

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
		BakedConvPos = dtoOutFurnace.BakedConvPos;
	}

	public OutFurnace(OutFurnaceStruct adsStruct)
	{
		StationCycleRID = adsStruct.CycleRID.ToRID();
		TS = adsStruct.TS.GetTimestamp();
		TwinCatStatus = adsStruct.Status;
		OutAnnounceID = adsStruct.MD.AnnounceID.ToRID();
		FTAPickUp = adsStruct.MD.PitFTA;
		PickUpTS = adsStruct.MD.PitPickup.GetTimestamp();
		DepositTS = adsStruct.MD.PitDeposit.GetTimestamp();
		InvalidPacket = adsStruct.MD.InvalidPacket;
		BakedConvPos = adsStruct.MD.BakedConvPos;
	}

	public override DTOOutFurnace ToDTO()
	{
		return new(this);
	}
}