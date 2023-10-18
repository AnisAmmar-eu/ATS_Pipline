using System.Data;
using System.Linq.Expressions;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.S3S4Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;

public partial class OutFurnace : Furnace, IBaseEntity<OutFurnace, DTOOutFurnace>
{
	public OutFurnace()
	{
	}

	public OutFurnace(OutFurnaceStruct adsStruct)
	{
		StationCycleRID = adsStruct.StationCycleRID.ToRID();
		AnnounceID = adsStruct.AnnounceID.ToRID();
		FTAPickUp = adsStruct.FTAPickUp;
		PickUpTS = adsStruct.PickUpTS.GetTimestamp();
		DepositTS = adsStruct.DepositTS.GetTimestamp();
		InvalidPacket = adsStruct.InvalidPacket;
	}

	public override DTOOutFurnace ToDTO()
	{
		return new DTOOutFurnace(this);
	}
	
	protected override async Task InheritedBuild(IAnodeUOW anodeUOW)
	{
		S3S4Cycle? cycle = await anodeUOW.StationCycle.GetBy(new Expression<Func<StationCycle, bool>>[]
		{
			cycle => cycle.RID == StationCycleRID
		}, withTracking: false) as S3S4Cycle;
		if (cycle == null)
			throw new InvalidConstraintException(StationCycleRID + " cycle is not a S3S4Cycle as expected");
		cycle.OutFurnacePacket = this;
		cycle.OutFurnaceID = ID;
		Status = PacketStatus.Completed;
		cycle.OutFurnaceStatus = Status;
		StationCycle = cycle;
		anodeUOW.StationCycle.Update(cycle);
	}
}