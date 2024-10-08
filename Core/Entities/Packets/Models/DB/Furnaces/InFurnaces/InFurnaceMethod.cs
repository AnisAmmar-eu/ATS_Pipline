using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.Structs;

namespace Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;

public partial class InFurnace
{
	public InFurnace()
	{
	}

	public InFurnace(DTOInFurnace dtoInFurnace) : base(dtoInFurnace)
	{
		InAnnounceID = dtoInFurnace.InAnnounceID;
		OriginID = dtoInFurnace.OriginID;
		PackPosition = dtoInFurnace.PackPosition;
		PalletSide = dtoInFurnace.PalletSide;
		PitNumber = dtoInFurnace.PitNumber;
		PitSectionNumber = dtoInFurnace.PitSectionNumber;
		PitHeight = dtoInFurnace.PitHeight;
		FTAPlace = dtoInFurnace.FTAPlace;
		GreenConvPos = dtoInFurnace.GreenConvPos;
		PitLoadTS = dtoInFurnace.PitLoadTS;
	}

	public InFurnace(InFurnaceStruct adsStruct)
	{
		StationCycleRID = adsStruct.CycleRID.ToRID();
		TS = adsStruct.TS.GetTimestamp();
		TwinCatStatus = adsStruct.Status;
		InAnnounceID = adsStruct.MD.AnnounceID.ToRID();
		OriginID = adsStruct.MD.OriginID;
		PackPosition = adsStruct.MD.PackPosition;
		PalletSide = adsStruct.MD.PalletSide;
		PitNumber = adsStruct.MD.PitNumber;
		PitSectionNumber = adsStruct.MD.PitSectionNumber;
		PitHeight = adsStruct.MD.PitHeight;
		FTAPlace = adsStruct.MD.PitFTA;
		GreenConvPos = adsStruct.MD.GreenConvPos;
		PitLoadTS = adsStruct.MD.PitLoadTS.GetTimestamp();
	}

	public override DTOInFurnace ToDTO() => new(this);
}