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

	public InFurnace(InFurnaceStruct adsStruct)
	{
		StationCycleRID = adsStruct.StationCycleRID.ToRID();
		AnnounceID = adsStruct.AnnounceID.ToRID();
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

	protected override async Task InheritedBuild(IAnodeUOW anodeUOW)
	{
		S3S4Cycle? cycle = await anodeUOW.StationCycle.GetBy(new Expression<Func<StationCycle, bool>>[]
		{
			cycle => cycle.RID == StationCycleRID
		}, withTracking: false) as S3S4Cycle;
		if (cycle == null)
			throw new InvalidConstraintException(StationCycleRID + " cycle is not a S3S4Cycle as expected");
		cycle.InFurnacePacket = this;
		cycle.InFurnaceID = ID;
		Status = PacketStatus.Completed;
		cycle.InFurnaceStatus = Status;
		StationCycle = cycle;
		anodeUOW.StationCycle.Update(cycle);
	}
}