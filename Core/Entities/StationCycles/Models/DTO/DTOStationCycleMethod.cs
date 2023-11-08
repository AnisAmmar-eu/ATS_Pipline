using Core.Entities.StationCycles.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO;

public partial class DTOStationCycle : DTOBaseEntity, IDTO<StationCycle, DTOStationCycle>
{
	public DTOStationCycle()
	{
	}

	public DTOStationCycle(StationCycle stationCycle)
	{
		ID = stationCycle.ID;
		TS = stationCycle.TS;

		StationID = stationCycle.StationID;
		AnodeType = stationCycle.AnodeType;
		RID = stationCycle.RID;
		Status = stationCycle.Status;
		TSClosed = stationCycle.TSClosed;
		SignStatus1 = (int)stationCycle.SignStatus1;
		SignStatus2 = (int)stationCycle.SignStatus2;

		AnnouncementStatus = stationCycle.AnnouncementStatus;
		AnnouncementID = stationCycle.AnnouncementID;
		AnnouncementPacket = stationCycle.AnnouncementPacket?.ToDTO();

		DetectionStatus = stationCycle.DetectionStatus;
		DetectionID = stationCycle.DetectionID;
		DetectionPacket = stationCycle.DetectionPacket?.ToDTO();

		ShootingStatus = stationCycle.ShootingStatus;
		ShootingID = stationCycle.ShootingID;
		ShootingPacket = stationCycle.ShootingPacket?.ToDTO();

		AlarmListStatus = stationCycle.AlarmListStatus;
		AlarmListID = stationCycle.AlarmListID;
		AlarmListPacket = stationCycle.AlarmListPacket?.ToDTO();
	}

	public override StationCycle ToModel()
	{
		return new StationCycle(this);
	}
}