using Core.Entities.Packets.Models.DB.Detections;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Repositories;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
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
			throw new InvalidOperationException("Staion cycle with RID: " + stationCycle.RID +
			                                    " does not have a detection packet for measurement");
		Detection detection = (await AnodeUOW.Packet.GetById((int)stationCycle.DetectionID) as Detection)!;
		// TODO Read variable
		/*
		AdsClient tcClient = new();
		while (!tcClient.IsConnected)
			tcClient.Connect(CycleAdsUtils.AdsPort);
			*/
		// temp
		detection.MeasuredType = AnodeTypeDict.D20;
		stationCycle.AnodeType = detection.MeasuredType;
		detection.IsMismatched = true;
		await AnodeUOW.StartTransaction();
		AnodeUOW.Packet.Update(detection);
		AnodeUOW.StationCycle.Update(stationCycle);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}
}