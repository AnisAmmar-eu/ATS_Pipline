using System.Text.Json.Serialization;
using Core.Entities.Anodes.Models.DTO;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Entities.Packets.Models.DTO.MetaDatas;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO.LoadableCycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.StationCycles.Models.DTO;

[JsonDerivedType(typeof(DTOLoadableCycle))]
[JsonDerivedType(typeof(DTOMatchingCycle))]
public partial class DTOStationCycle :
	DTOBaseEntity,
	IDTO<StationCycle, DTOStationCycle>,
	IBindableFromHttpContext<DTOStationCycle>
{
	public int StationID { get; set; } = Station.ID;
	public string AnodeType { get; set; } = string.Empty;
	public string RID { get; set; } = string.Empty;
	public string SerialNumber { get; set; } = "NA";
	public PacketStatus Status { get; set; } = PacketStatus.Initialized;
	public string CycleType { get; set; } = string.Empty;
	public DateTimeOffset? TSFirstShooting { get; set; }
	public SignMatchStatus SignStatus1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus SignStatus2 { get; set; } = SignMatchStatus.NA;

	public int? MetaDataID { get; set; }
	public DTOMetaData? MetaDataPacket { get; set; }

	public int Picture1Status { get; set; }
	public int? Shooting1ID { get; set; }
	public DTOShooting? Shooting1Packet { get; set; }

	public int Picture2Status { get; set; }
	public int? Shooting2ID { get; set; }
	public DTOShooting? Shooting2Packet { get; set; }

	public int? AlarmListID { get; set; }
	public DTOAlarmList? AlarmListPacket { get; set; }
	public DTOAnode? Anode { get; set; }

	public int InstanceMatchID { get; set; }
	public int NbActiveAlarms { get; set; }
	public bool HasPlug { get; set; }
}