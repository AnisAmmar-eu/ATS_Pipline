using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.Announcements;
using Core.Entities.Packets.Models.DB.Detections;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DTO;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB;

public partial class StationCycle : BaseEntity, IBaseEntity<StationCycle, DTOStationCycle>
{
	public string AnodeType { get; set; }
	public string RID { get; set; }
	public string Status { get; set; } = CycleStatus.Initialized;
	public DateTimeOffset? TSClosed { get; set; }

	public string? AnnouncementStatus { get; set; }
	public int? AnnouncementID { get; set; }
	public Announcement? AnnouncementPacket { get; set; }

	public string? DetectionStatus { get; set; }
	public int? DetectionID { get; set; }
	public Detection? DetectionPacket { get; set; }

	public string? ShootingStatus { get; set; }
	public int? ShootingID { get; set; }
	public Shooting? ShootingPacket { get; set; }

	public string? AlarmListStatus { get; set; }
	public int? AlarmListID { get; set; }
	public AlarmList? AlarmListPacket { get; set; }
}