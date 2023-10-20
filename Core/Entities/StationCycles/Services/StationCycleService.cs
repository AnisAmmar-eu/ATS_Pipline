using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Detections;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Repositories;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using TwinCAT;
using TwinCAT.Ads;

namespace Core.Entities.StationCycles.Services;

public class StationCycleService : ServiceBaseEntity<IStationCycleRepository, StationCycle, DTOStationCycle>,
	IStationCycleService
{
	public StationCycleService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task UpdateDetectionWithMeasure(StationCycle stationCycle)
	{
		if (stationCycle.DetectionID == null)
			throw new InvalidOperationException("Station cycle with RID: " + stationCycle.RID +
			                                    " does not have a detection packet for measurement");
		Detection detection = (await AnodeUOW.Packet.GetById(stationCycle.DetectionID.Value) as Detection)!;
		AdsClient tcClient = new();
		tcClient.Connect(ADSUtils.AdsPort);
		if (!tcClient.IsConnected)
			throw new AdsException("Could not connect to TwinCat");
		uint handle = tcClient.CreateVariableHandle(ADSUtils.MeasurementVariable);
		MeasureStruct measure = tcClient.ReadAny<MeasureStruct>(handle);
		detection.AnodeSize = measure.AnodeSize;
		detection.MeasuredType = measure.AnodeType == 1 ? AnodeTypeDict.DX : AnodeTypeDict.D20;
		detection.IsSameType = measure.IsSameType;
		stationCycle.AnodeType = detection.MeasuredType;
		detection.Status = PacketStatus.Completed;
		await AnodeUOW.StartTransaction();
		AnodeUOW.Packet.Update(detection);
		AnodeUOW.StationCycle.Update(stationCycle);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}
}