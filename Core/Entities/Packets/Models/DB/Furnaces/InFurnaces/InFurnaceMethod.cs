using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;

public partial class InFurnace : Furnace, IBaseEntity<InFurnace, DTOInFurnace>
{
	public InFurnace()
	{
	}

	public InFurnace(InFurnaceStruct adsStruct)
	{
		StationCycleRID = adsStruct.StationCycleRID.ToRID();
		AnnounceID = adsStruct.AnnounceID;
		OriginID = adsStruct.OriginID;
		PackPosition = adsStruct.PackPosition;
		PalletSide = adsStruct.PalletSide;
		PitNumber = adsStruct.PitNumber;
		PitSectionNumber = adsStruct.PitSectionNumber;
		PitHeight = adsStruct.PitHeight;
		FTAPlace = adsStruct.FTAPlace;
		FTASuck = adsStruct.FTASuck;
		GreenConvPos = adsStruct.GreenConvPos;
		BakedConvPos = adsStruct.BakedConvPos;
		PitLoadTS = adsStruct.PitLoadTS.GetTimestamp();
	}

	public override DTOInFurnace ToDTO()
	{
		return new DTOInFurnace(this);
	}
}