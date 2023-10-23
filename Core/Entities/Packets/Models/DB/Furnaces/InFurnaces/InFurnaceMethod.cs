using System.Data;
using System.Linq.Expressions;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.S3S4Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;

public partial class InFurnace : Furnace, IBaseEntity<InFurnace, DTOInFurnace>
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
		FTASuck = dtoInFurnace.FTASuck;
		GreenConvPos = dtoInFurnace.GreenConvPos;
		BakedConvPos = dtoInFurnace.BakedConvPos;
		PitLoadTS = dtoInFurnace.PitLoadTS;
	}

	public InFurnace(InFurnaceStruct adsStruct)
	{
		StationCycleRID = adsStruct.StationCycleRID.ToRID();
		InAnnounceID = adsStruct.AnnounceID.ToRID();
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

	protected override void FurnaceAssign(S3S4Cycle cycle)
	{
		cycle.InFurnaceID = ID;
		cycle.InFurnaceStatus = Status;
		cycle.InFurnacePacket = this;
	}
}