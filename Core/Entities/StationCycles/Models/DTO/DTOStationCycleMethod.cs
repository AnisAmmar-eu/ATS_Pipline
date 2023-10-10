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
		
		AnodeType = stationCycle.AnodeType;
		AnnounceID = stationCycle.AnnounceID;
		RID = stationCycle.RID;
		Status = stationCycle.Status;
		TSClosed = stationCycle.TSClosed;

		AnnouncementStatus = stationCycle.AnnouncementStatus;
		AnnouncementID = stationCycle.AnnouncementID;
		AnnouncementPacket = stationCycle.AnnouncementPacket.ToDTO();

		DetectionStatus = stationCycle.DetectionStatus;
		DetectionID = stationCycle.DetectionID;
		DetectionPacket = stationCycle.DetectionPacket.ToDTO();

		ShootingStatus = stationCycle.ShootingStatus;
		ShootingID = stationCycle.ShootingID;
		ShootingPacket = stationCycle.ShootingPacket.ToDTO();

		AlarmListStatus = stationCycle.AlarmListStatus;
		AlarmListID = stationCycle.AlarmListID;
		AlarmListPacket = stationCycle.AlarmListPacket.ToDTO();
	}
}