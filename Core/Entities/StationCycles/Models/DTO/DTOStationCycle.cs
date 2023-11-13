using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Entities.Packets.Models.DTO.Announcements;
using Core.Entities.Packets.Models.DTO.Detections;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO.Binders;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Core.Entities.StationCycles.Models.DTO;

[ModelBinder(typeof(DTOStationCycleBinder))]
public partial class DTOStationCycle : DTOBaseEntity, IDTO<StationCycle, DTOStationCycle>
{
	public int StationID { get; set; } = Station.ID;
	public string AnodeType { get; set; } = string.Empty;
	public string RID { get; set; } = string.Empty;
	public string Status { get; set; } = PacketStatus.Initialized;
	public string CycleType { get; set; } = string.Empty;
	public DateTimeOffset? TSClosed { get; set; }
	public SignMatchStatus SignStatus1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus SignStatus2 { get; set; } = SignMatchStatus.NA;

	public string? AnnouncementStatus { get; set; }
	public int? AnnouncementID { get; set; }
	public DTOAnnouncement? AnnouncementPacket { get; set; }

	public string? DetectionStatus { get; set; }
	public int? DetectionID { get; set; }
	public DTODetection? DetectionPacket { get; set; }

	public string? ShootingStatus { get; set; }
	public int? ShootingID { get; set; }
	public DTOShooting? ShootingPacket { get; set; }

	public string? AlarmListStatus { get; set; }
	public int? AlarmListID { get; set; }
	public DTOAlarmList? AlarmListPacket { get; set; }
}