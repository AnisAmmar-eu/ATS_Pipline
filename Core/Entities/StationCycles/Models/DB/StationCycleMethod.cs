using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.S1S2Cycles;
using Core.Entities.StationCycles.Models.DB.S3S4Cycles;
using Core.Entities.StationCycles.Models.DB.S5Cycles;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB;

public partial class StationCycle : BaseEntity, IBaseEntity<StationCycle, DTOStationCycle>
{
	protected StationCycle()
	{
	}

	public StationCycle(DTOStationCycle dtoStationCycle)
	{
		AnodeType = dtoStationCycle.AnodeType;
		StationID = dtoStationCycle.StationID;
		RID = dtoStationCycle.RID;
		Status = dtoStationCycle.Status;
		TSClosed = dtoStationCycle.TSClosed;
		SignStatus1 = (SignMatchStatus)dtoStationCycle.SignStatus1;
		SignStatus2 = (SignMatchStatus)dtoStationCycle.SignStatus2;

		AnnouncementStatus = dtoStationCycle.AnnouncementStatus;
		AnnouncementID = dtoStationCycle.AnnouncementID;
		AnnouncementPacket = dtoStationCycle.AnnouncementPacket?.ToModel();

		DetectionStatus = dtoStationCycle.DetectionStatus;
		DetectionID = dtoStationCycle.DetectionID;
		DetectionPacket = dtoStationCycle.DetectionPacket?.ToModel();

		ShootingStatus = dtoStationCycle.ShootingStatus;
		ShootingID = dtoStationCycle.ShootingID;
		ShootingPacket = dtoStationCycle.ShootingPacket?.ToModel();

		AlarmListStatus = dtoStationCycle.AlarmListStatus;
		AlarmListID = dtoStationCycle.AlarmListID;
		AlarmListPacket = dtoStationCycle.AlarmListPacket?.ToModel();
	}

	public override DTOStationCycle ToDTO()
	{
		return new DTOStationCycle(this);
	}

	public static StationCycle Create()
	{
		if (Station.Type == StationType.S1S2)
			return new S1S2Cycle();
		if (Station.Type == StationType.S3S4)
			return new S3S4Cycle();
		return new S5Cycle();
	}

	public ReducedStationCycle Reduce()
	{
		return new ReducedStationCycle
		{
			ID = ID,
			RID = RID,
			AnodeSize = DetectionPacket?.AnodeSize,
			AnodeType = AnodeType,
			ShootingTS = ShootingPacket?.ShootingTS
		};
	}
}