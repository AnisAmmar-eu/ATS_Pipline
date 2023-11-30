using System.Linq.Expressions;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.Detections;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.StationCycles.Models.DB;
using Core.Shared.Exceptions;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB.Detections;

public partial class Detection
{
	public Detection()
	{
	}

	public Detection(DTODetection dtoDetection) : base(dtoDetection)
	{
		AnodeSize = dtoDetection.AnodeSize;
		IsSameType = dtoDetection.IsSameType;
		MeasuredType = dtoDetection.MeasuredType;
	}

	public Detection(DetectionStruct detectionStruct)
	{
		StationCycleRID = detectionStruct.StationCycleRID.ToRID();
		AnodeSize = detectionStruct.AnodeHigh;
		// Other fields are completed later.
	}

	public override DTODetection ToDTO()
	{
		return new(this);
	}

	protected override async Task InheritedBuild(IAnodeUOW anodeUOW)
	{
		StationCycle stationCycle;
		try
		{
			stationCycle = await anodeUOW.StationCycle.GetBy(
				new Expression<Func<StationCycle, bool>>[] {
					cycle => cycle.RID == StationCycleRID
					},
				withTracking: false);
			stationCycle.DetectionStatus = PacketStatus.Initialized;
			stationCycle.DetectionID = ID;
			stationCycle.DetectionPacket = this;
			anodeUOW.StationCycle.Update(stationCycle);
		}
		catch (EntityNotFoundException)
		{
			stationCycle = StationCycle.Create();
			stationCycle.RID = StationCycleRID;
			stationCycle.DetectionStatus = PacketStatus.Initialized;
			stationCycle.DetectionID = ID;
			stationCycle.DetectionPacket = this;
			await anodeUOW.StationCycle.Add(stationCycle);
		}

		Status = PacketStatus.Initialized;
		StationCycle = stationCycle;
	}
}