using Core.Entities.Anodes.Models.DB;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.MetaDatas;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DTO;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB;

public partial class StationCycle : BaseEntity, IBaseEntity<StationCycle, DTOStationCycle>
{
	public int StationID { get; set; } = Station.ID;
	public string AnodeType { get; set; } = string.Empty;
	public string RID { get; set; } = string.Empty;
	public string SerialNumber { get; set; } = "NA";
	public PacketStatus Status { get; set; } = PacketStatus.Initialized;
	public DateTimeOffset? TSFirstShooting { get; set; }
	public SignMatchStatus SignStatus1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus SignStatus2 { get; set; } = SignMatchStatus.NA;

	public int? MetaDataID { get; set; }
	public MetaData? MetaDataPacket { get; set; }

	public int Picture1Status { get; set; }
	public int? Shooting1ID { get; set; }
	public Shooting? Shooting1Packet { get; set; }

	public int Picture2Status { get; set; }
	public int? Shooting2ID { get; set; }
	public Shooting? Shooting2Packet { get; set; }

	public int? AlarmListID { get; set; }
	public AlarmList? AlarmListPacket { get; set; }

	public Anode? Anode { get; set; }

	public int NbActiveAlarms { get; set; }
	public bool HasPlug { get; set; }
}